using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nimbus.Core;
using Newtonsoft.Json.Linq;

namespace Nimbus.Save;

/// <summary>
/// Concrete implementation of the SaveService.
/// Orchestrates the serialization/deserialization of registered save slices.
/// </summary>
public sealed class SaveService : ISaveService, IInitializable
{
    private const int SaveEnvelopeVersion = 1;
    private const string DefaultSaveName = "save_data";

    private readonly ISaveStorage _storage;
    private readonly Dictionary<string, ISaveSlice> _slices = new();
    private JObject? _latestLoadedSlices;
    private string _currentProfileId;

    /// <inheritdoc />
    public bool IsSaving { get; private set; }

    /// <inheritdoc />
    public bool IsLoading { get; private set; }

    /// <inheritdoc />
    public bool IsLoaded { get; private set; }

    /// <inheritdoc />
    public string CurrentProfileId => _currentProfileId;

    /// <summary>
    /// Initializes a new instance of the <see cref="SaveService"/> class.
    /// </summary>
    /// <param name="storage">The storage provider to use.</param>
    /// <param name="initialProfileId">The initial profile ID to load.</param>
    public SaveService(ISaveStorage storage, string initialProfileId = DefaultSaveName)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _currentProfileId = initialProfileId;
    }

    /// <summary>
    /// Default constructor for common cases.
    /// </summary>
    public SaveService() : this(new FileSaveStorage(), DefaultSaveName)
    {
    }

    /// <inheritdoc />
    public async UniTask InitializeAsync(CancellationToken cancellationToken)
    {
        Log.Save.Info($"Initializing SaveService with profile: {_currentProfileId}");
        await LoadAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async UniTask<IEnumerable<SaveProfile>> GetAvailableProfilesAsync()
    {
        var names = await _storage.ListNamesAsync();
        var profiles = new List<SaveProfile>();

        foreach (var name in names)
        {
            var lastSaved = await _storage.GetLastWriteTimeAsync(name);
            profiles.Add(new SaveProfile(name, lastSaved));
        }

        return profiles;
    }

    /// <inheritdoc />
    public async UniTask SwitchProfileAsync(string profileId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(profileId))
        {
            throw new ArgumentException("Profile ID cannot be null or empty.", nameof(profileId));
        }

        if (_currentProfileId == profileId && IsLoaded)
        {
            return;
        }

        Log.Save.Info($"Switching save profile from '{_currentProfileId}' to '{profileId}'.");
        _currentProfileId = profileId;
        IsLoaded = false;

        await LoadAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async UniTask DeleteProfileAsync(string profileId)
    {
        if (string.IsNullOrEmpty(profileId))
        {
            throw new ArgumentException("Profile ID cannot be null or empty.", nameof(profileId));
        }

        Log.Save.Info($"Deleting save profile '{profileId}'.");
        await _storage.DeleteAsync(profileId);

        if (_currentProfileId == profileId)
        {
            IsLoaded = false;
        }
    }

    /// <inheritdoc />
    public void RegisterSlice(ISaveSlice slice)
    {
        if (slice == null)
        {
            throw new ArgumentNullException(nameof(slice));
        }

        if (_slices.ContainsKey(slice.Key))
        {
            Log.Save.Warning($"Save slice with key '{slice.Key}' is already registered. Overwriting.");
        }

        _slices[slice.Key] = slice;

        if (IsLoaded && _latestLoadedSlices != null && _latestLoadedSlices.TryGetValue(slice.Key, out var token))
        {
            if (token is JObject sliceNode)
            {
                ApplySliceData(slice, sliceNode);
            }
        }
    }

    /// <inheritdoc />
    public void UnregisterSlice(string key)
    {
        _slices.Remove(key);
    }

    /// <inheritdoc />
    public ISaveSlice? GetSlice(string key)
    {
        return _slices.TryGetValue(key, out var slice) ? slice : null;
    }

    /// <inheritdoc />
    public IEnumerable<ISaveSlice> GetAllSlices()
    {
        return _slices.Values;
    }

    /// <inheritdoc />
    public async UniTask SaveAsync(CancellationToken cancellationToken = default)
    {
        if (IsSaving)
        {
            Log.Save.Warning("Save operation already in progress. Skipping.");
            return;
        }

        IsSaving = true;
        EventBus.Publish(new SaveStarted());
        Log.Save.Info("Starting save operation.");

        try
        {
            await InternalSaveAsync(cancellationToken);
            Log.Save.Info("Save operation completed successfully.");
            EventBus.Publish(new SaveCompleted());
        }
        catch (Exception ex)
        {
            Log.Save.Error($"Save operation failed: {ex.Message}");
            EventBus.Publish(new SaveFailed(ex.Message));
        }
        finally
        {
            IsSaving = false;
        }
    }

    /// <inheritdoc />
    public async UniTask LoadAsync(CancellationToken cancellationToken = default)
    {
        if (IsLoading)
        {
            Log.Save.Warning("Load operation already in progress. Skipping.");
            return;
        }

        IsLoading = true;
        Log.Save.Info("Starting load operation.");

        try
        {
            await InternalLoadAsync(cancellationToken);
            IsLoaded = true;
            EventBus.Publish(new LoadCompleted());
        }
        catch (Exception ex)
        {
            Log.Save.Error($"Load operation failed: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async UniTask InternalSaveAsync(CancellationToken cancellationToken)
    {
        var root = new JObject
        {
            ["version"] = SaveEnvelopeVersion,
            ["profile"] = _currentProfileId
        };

        var slicesNode = _latestLoadedSlices != null
            ? new JObject(_latestLoadedSlices)
            : new JObject();

        foreach (var slice in _slices.Values)
        {
            slicesNode[slice.Key] = new JObject
            {
                ["version"] = slice.Version,
                ["data"] = slice.Serialize()
            };
        }

        root["slices"] = slicesNode;
        await _storage.WriteAsync(_currentProfileId, root);
    }

    private async UniTask InternalLoadAsync(CancellationToken cancellationToken)
    {
        foreach (var slice in _slices.Values)
        {
            slice.Deserialize(new JObject());
        }

        var root = await _storage.ReadAsync(_currentProfileId);
        if (root == null)
        {
            _latestLoadedSlices = null;
            Log.Save.Info("No save data found. Initializing with defaults.");
            return;
        }

        PopulateFromRoot(root);
        Log.Save.Info("Load operation completed successfully.");
    }

    private void PopulateFromRoot(JObject root)
    {
        var savedEnvelopeVersion = root["version"]?.Value<int>() ?? 0;
        if (savedEnvelopeVersion < SaveEnvelopeVersion)
        {
            Log.Save.Info($"Migrating save envelope from version {savedEnvelopeVersion} to {SaveEnvelopeVersion}.");
        }

        _latestLoadedSlices = root["slices"] as JObject;
        if (_latestLoadedSlices == null)
        {
            Log.Save.Warning("Save data found but 'slices' node is missing. Corrupted?");
            return;
        }

        foreach (var property in _latestLoadedSlices.Properties())
        {
            if (_slices.TryGetValue(property.Name, out var slice) && property.Value is JObject sliceNode)
            {
                ApplySliceData(slice, sliceNode);
            }
        }
    }

    private void ApplySliceData(ISaveSlice slice, JObject sliceNode)
    {
        var savedVersion = sliceNode["version"]?.Value<int>() ?? 0;
        var data = sliceNode["data"] as JObject;
        if (data == null)
        {
            return;
        }

        if (savedVersion < slice.Version)
        {
            Log.Save.Info($"Migrating slice '{slice.Key}' from version {savedVersion} to {slice.Version}.");
            slice.Migrate(savedVersion, data);
        }
        else if (savedVersion > slice.Version)
        {
            Log.Save.Warning($"Save data for slice '{slice.Key}' has version {savedVersion}, which is newer than registered slice version {slice.Version}. Data might be incompatible.");
        }

        slice.Deserialize(data);
    }
}
