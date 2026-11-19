namespace Nimbus.Quests.Events;

public readonly struct QuestCompleted
{
    public readonly string QuestId;

    public QuestCompleted(string questId)
    {
        QuestId = questId;
    }
}

public readonly struct QuestClaimed
{
    public readonly string QuestId;

    public QuestClaimed(string questId)
    {
        QuestId = questId;
    }
}
