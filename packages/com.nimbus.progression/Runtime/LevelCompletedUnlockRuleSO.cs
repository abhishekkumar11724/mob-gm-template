using UnityEngine;

namespace Nimbus.Progression;

/// <summary>
/// An unlock rule that checks if a specific level has been completed.
/// </summary>
[CreateAssetMenu(fileName = "LevelCompletedUnlockRule", menuName = "Nimbus/Progression/Rules/Level Completed")]
public sealed class LevelCompletedUnlockRuleSO : UnlockRuleSO
{
    [SerializeField] private string _levelId = string.Empty;
    [SerializeField] private int _requiredStars = 0;

    /// <summary>
    /// Gets the unique identifier of the required level.
    /// </summary>
    public string LevelId => _levelId;

    /// <summary>
    /// Gets the required number of stars.
    /// </summary>
    public int RequiredStars => _requiredStars;

    /// <inheritdoc />
    public override bool IsSatisfied(IProgressionService progression)
    {
        var record = progression.GetLevelRecord(_levelId);

        if (!record.HasValue)
        {
            return false;
        }

        return record.Value.Stars >= _requiredStars;
    }
}
