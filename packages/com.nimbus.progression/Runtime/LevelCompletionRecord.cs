namespace Nimbus.Progression;

public readonly struct LevelCompletionRecord
{
    public readonly int BestScore;
    public readonly int Stars;
    public readonly float Time;

    public LevelCompletionRecord(int bestScore, int stars, float time)
    {
        BestScore = bestScore;
        Stars = stars;
        Time = time;
    }
}
