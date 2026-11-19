using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Nimbus.Save;

/// <summary>
/// Abstract storage provider for save data (e.g., local file system, cloud storage).
/// </summary>
public interface ISaveStorage
{
    /// <summary>
    /// Checks if a save file with the given name exists.
    /// </summary>
    /// <param name="name">The name of the save (e.g., "profile_0").</param>
    /// <returns>UniTask(bool).</returns>
    UniTask<bool> ExistsAsync(string name);

    /// <summary>
    /// Reads a save file and returns its content as a JObject.
    /// </summary>
    /// <param name="name">The name of the save.</param>
    /// <returns>UniTask(JObject) containing the save data, or null if not found.</returns>
    UniTask<JObject?> ReadAsync(string name);

    /// <summary>
    /// Writes the given JObject data to a save file.
    /// </summary>
    /// <param name="name">The name of the save.</param>
    /// <param name="data">The data to write.</param>
    /// <returns>UniTask.</returns>
    UniTask WriteAsync(string name, JObject data);

    /// <summary>
    /// Deletes a save file.
    /// </summary>
    /// <param name="name">The name of the save to delete.</param>
    /// <returns>UniTask.</returns>
    UniTask DeleteAsync(string name);

    /// <summary>
    /// Lists the names of all available saves.
    /// </summary>
    /// <returns>UniTask containing a collection of save names.</returns>
    UniTask<System.Collections.Generic.IEnumerable<string>> ListNamesAsync();

    /// <summary>
    /// Gets the last write time of a save file.
    /// </summary>
    /// <param name="name">The name of the save.</param>
    /// <returns>UniTask containing the last write time.</returns>
    UniTask<System.DateTime> GetLastWriteTimeAsync(string name);
}
