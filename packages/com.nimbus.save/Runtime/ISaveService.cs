using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nimbus.Core;

namespace Nimbus.Save;

/// <summary>
/// Orchestrates save and load operations for all registered save slices.
/// </summary>
public interface ISaveService : IService
{
    /// <summary>
    /// Gets a value indicating whether a save operation is currently in progress.
    /// </summary>
    bool IsSaving { get; }

    /// <summary>
    /// Gets a value indicating whether a load operation is currently in progress.
    /// </summary>
    bool IsLoading { get; }

    /// <summary>
    /// Gets a value indicating whether the save data has been successfully loaded at least once.
    /// </summary>
    bool IsLoaded { get; }

    /// <summary>
    /// Gets the unique identifier of the current active profile.
    /// </summary>
    string CurrentProfileId { get; }

    /// <summary>
    /// Gets all available save profiles.
    /// </summary>
    /// <returns>A collection of available profiles.</returns>
    UniTask<IEnumerable<SaveProfile>> GetAvailableProfilesAsync();

    /// <summary>
    /// Switches the active profile and reloads the data.
    /// </summary>
    /// <param name="profileId">The ID of the profile to switch to.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>UniTask.</returns>
    UniTask SwitchProfileAsync(string profileId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a profile's save data.
    /// </summary>
    /// <param name="profileId">The ID of the profile to delete.</param>
    /// <returns>UniTask.</returns>
    UniTask DeleteProfileAsync(string profileId);

    /// <summary>
    /// Registers a save slice with the service.
    /// </summary>
    /// <param name="slice">The slice to register.</param>
    void RegisterSlice(ISaveSlice slice);

    /// <summary>
    /// Unregisters a save slice from the service.
    /// </summary>
    /// <param name="key">The unique key of the slice to unregister.</param>
    void UnregisterSlice(string key);

    /// <summary>
    /// Gets a registered save slice by its key.
    /// </summary>
    /// <param name="key">The unique key of the slice.</param>
    /// <returns>The slice if found, otherwise null.</returns>
    ISaveSlice? GetSlice(string key);

    /// <summary>
    /// Gets all registered save slices.
    /// </summary>
    /// <returns>A collection of all registered slices.</returns>
    IEnumerable<ISaveSlice> GetAllSlices();

    /// <summary>
    /// Triggers an asynchronous save operation for all registered slices.
    /// Fires SaveStarted, then SaveCompleted or SaveFailed events via EventBus.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>UniTask.</returns>
    UniTask SaveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Triggers an asynchronous load operation for all registered slices.
    /// Fires LoadCompleted event via EventBus on success.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>UniTask.</returns>
    UniTask LoadAsync(CancellationToken cancellationToken = default);
}
