namespace Nimbus.Quests;

public interface IQuestObjective
{
    bool Matches(QuestEvent questEvent);

    int GetProgressDelta(QuestEvent questEvent, int currentProgress);

    string GetDescriptionFormat();
}
