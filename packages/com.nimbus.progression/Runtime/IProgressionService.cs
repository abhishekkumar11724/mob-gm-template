using Nimbus.Core;

namespace Nimbus.Progression;

/// <summary>
/// Provides access to player progression data, including levels, XP, and level results.
/// </summary>
public interface IProgressionService : IService
{
    /// <summary>
    /// Gets the current player level.
    /// </summary>
    int CurrentLevel { get; }

    /// <summary>
    /// Gets the current XP in the current level.
    /// </summary>
    int CurrentXP { get; }

    /// <summary>
    /// Gets the total XP required to reach the next level.
    /// </summary>
    int XPToNextLevel { get; }

    /// <summary>
    /// Adds XP to the player's total.
    /// Handles leveling up if the threshold is reached.
    /// </summary>
    /// <param name="amount">The amount of XP to add.</param>
    void AddXP(int amount);

    /// <summary>
    /// Checks if a specific unlock rule is currently satisfied.
    /// </summary>
    /// <param name="rule">The unlock rule to check.</param>
    /// <returns>True if the rule is satisfied, otherwise false.</returns>
    bool IsUnlocked(UnlockRuleSO rule);

    /// <summary>
    /// Records the completion of a level.
    /// </summary>
    /// <param name="levelId">The unique identifier for the level.</param>
    /// <param name="result">The results of the level completion.</param>
    void CompleteLevel(string levelId, LevelCompletionRecord result);

    /// <summary>
    /// Gets the best completion record for a specific level.
    /// </summary>
    /// <param name="levelId">The unique identifier for the level.</param>
    /// <returns>An optional containing the level completion record if it exists.</returns>
    Optional<LevelCompletionRecord> GetLevelRecord(string levelId);
}
