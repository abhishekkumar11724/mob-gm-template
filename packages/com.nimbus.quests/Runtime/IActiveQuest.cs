namespace Nimbus.Quests;

public interface IActiveQuest
{
    string Id { get; }

    int Progress { get; }

    int Target { get; }

    QuestStatus Status { get; }

    bool IsCompleted => Status == QuestStatus.Completed || Status == QuestStatus.Claimed;

    bool IsClaimed => Status == QuestStatus.Claimed;

    float NormalizedProgress => Target > 0 ? (float)Progress / Target : 0f;
}
