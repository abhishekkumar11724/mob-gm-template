using UnityEngine;

namespace Nimbus.Progression;

/// <summary>
/// An unlock rule that checks if the player has reached a specific level.
/// </summary>
[CreateAssetMenu(fileName = "ReachedLevelUnlockRule", menuName = "Nimbus/Progression/Rules/Reached Level")]
public sealed class ReachedLevelUnlockRuleSO : UnlockRuleSO
{
    [SerializeField] private int _requiredLevel = 1;

    /// <summary>
    /// Gets the required level.
    /// </summary>
    public int RequiredLevel => _requiredLevel;

    /// <inheritdoc />
    public override bool IsSatisfied(IProgressionService progression)
    {
        return progression.CurrentLevel >= _requiredLevel;
    }
}
