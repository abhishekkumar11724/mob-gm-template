using System;
using System.Collections.Generic;
using Nimbus.Core;
using Nimbus.Quests.Events;

namespace Nimbus.Quests;

public sealed class QuestService : IQuestService
{
    private readonly List<ActiveQuest> _activeQuests = new();

    public IReadOnlyList<IActiveQuest> GetActiveQuests()
    {
        return _activeQuests;
    }

    public bool TryClaim(string questId)
    {
        if (string.IsNullOrEmpty(questId))
        {
            throw new ArgumentException("Quest ID cannot be null or empty", nameof(questId));
        }

        var quest = _activeQuests.Find(q => q.Id == questId);

        if (quest == null || quest.Status != QuestStatus.Completed)
        {
            return false;
        }

        quest.SetStatus(QuestStatus.Claimed);
        EventBus.Publish(new QuestClaimed(questId));

        return true;
    }

    public void Track(QuestEvent questEvent)
    {
        foreach (var quest in _activeQuests)
        {
            if (quest.Status != QuestStatus.Active)
            {
                continue;
            }

            if (quest.Objective.Matches(questEvent))
            {
                int delta = quest.Objective.GetProgressDelta(questEvent, quest.Progress);
                quest.AddProgress(delta);

                if (quest.Progress >= quest.Target)
                {
                    quest.SetStatus(QuestStatus.Completed);
                    EventBus.Publish(new QuestCompleted(quest.Id));
                }
            }
        }
    }

    public void AddQuest(string id, IQuestObjective objective, int target)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException("Quest ID cannot be null or empty", nameof(id));
        }

        if (objective == null)
        {
            throw new ArgumentNullException(nameof(objective));
        }

        _activeQuests.Add(new ActiveQuest(id, objective, target));
    }

    private sealed class ActiveQuest : IActiveQuest
    {
        public string Id { get; }

        public IQuestObjective Objective { get; }

        public int Progress { get; private set; }

        public int Target { get; }

        public QuestStatus Status { get; private set; }

        public ActiveQuest(string id, IQuestObjective objective, int target)
        {
            Id = id;
            Objective = objective;
            Target = target;
            Status = QuestStatus.Active;
        }

        public void AddProgress(int delta)
        {
            Progress = Math.Min(Progress + delta, Target);
        }

        public void SetStatus(QuestStatus status)
        {
            Status = status;
        }
    }
}
