using UnityEngine;

namespace Nimbus.Progression;

/// <summary>
/// Abstract base class for all unlock rules.
/// </summary>
public abstract class UnlockRuleSO : ScriptableObject
{
    /// <summary>
    /// Checks if this unlock rule is satisfied.
    /// </summary>
    /// <param name="progression">The progression service to check against.</param>
    /// <returns>True if the rule is satisfied, otherwise false.</returns>
    public abstract bool IsSatisfied(IProgressionService progression);
}
