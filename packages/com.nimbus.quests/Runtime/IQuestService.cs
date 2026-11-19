using System.Collections.Generic;
using Nimbus.Core;

namespace Nimbus.Quests;

public interface IQuestService : IService
{
    IReadOnlyList<IActiveQuest> GetActiveQuests();

    bool TryClaim(string questId);

    void Track(QuestEvent questEvent);
}
