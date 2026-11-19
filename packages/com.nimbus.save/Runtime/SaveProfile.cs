using System;

namespace Nimbus.Save;

/// <summary>
/// Represents a save profile/slot with basic metadata.
/// </summary>
public sealed class SaveProfile
{
    /// <summary>
    /// Gets the unique identifier/name of the profile.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the date and time when the profile was last saved.
    /// </summary>
    public DateTime LastSaved { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SaveProfile"/> class.
    /// </summary>
    /// <param name="id">The profile identifier.</param>
    /// <param name="lastSaved">The last saved timestamp.</param>
    public SaveProfile(string id, DateTime lastSaved)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        LastSaved = lastSaved;
    }
}
