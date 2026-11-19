using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Nimbus.Save;

/// <summary>
/// Default implementation of ISaveStorage using local file system.
/// </summary>
public sealed class FileSaveStorage : ISaveStorage
{
    private readonly string _directory;

    public FileSaveStorage(string directory)
    {
        _directory = directory;
        if (!Directory.Exists(_directory))
        {
            Directory.CreateDirectory(_directory);
        }
    }

    public FileSaveStorage() : this(Application.persistentDataPath)
    {
    }

    public UniTask<bool> ExistsAsync(string name)
    {
        var path = GetPath(name);
        return UniTask.FromResult(File.Exists(path));
    }

    public async UniTask<JObject?> ReadAsync(string name)
    {
        var path = GetPath(name);
        if (!File.Exists(path))
        {
            return null;
        }

        try
        {
            // Using UniTask.RunOnThreadPool for blocking I/O to keep main thread free
            var json = await UniTask.RunOnThreadPool(() => File.ReadAllText(path, Encoding.UTF8));
            return JObject.Parse(json);
        }
        catch (System.Exception ex)
        {
            Nimbus.Core.Log.Save.Error($"Failed to read save file {path}: {ex.Message}");
            return null;
        }
    }

    public async UniTask WriteAsync(string name, JObject data)
    {
        var path = GetPath(name);
        var json = data.ToString(Formatting.None);

        try
        {
            await UniTask.RunOnThreadPool(() => File.WriteAllText(path, json, Encoding.UTF8));
        }
        catch (System.Exception ex)
        {
            Nimbus.Core.Log.Save.Error($"Failed to write save file {path}: {ex.Message}");
            throw;
        }
    }

    public UniTask DeleteAsync(string name)
    {
        var path = GetPath(name);
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        return UniTask.CompletedTask;
    }

    public UniTask<System.Collections.Generic.IEnumerable<string>> ListNamesAsync()
    {
        if (!Directory.Exists(_directory))
        {
            return UniTask.FromResult(System.Linq.Enumerable.Empty<string>());
        }

        var files = Directory.GetFiles(_directory, "*.json");
        var names = System.Linq.Enumerable.Select(files, Path.GetFileNameWithoutExtension);
        return UniTask.FromResult(names);
    }

    public UniTask<System.DateTime> GetLastWriteTimeAsync(string name)
    {
        var path = GetPath(name);
        if (!File.Exists(path))
        {
            return UniTask.FromResult(System.DateTime.MinValue);
        }

        return UniTask.FromResult(File.GetLastWriteTime(path));
    }

    private string GetPath(string name) => Path.Combine(_directory, $"{name}.json");
}
