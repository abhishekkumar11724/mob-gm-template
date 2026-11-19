namespace Nimbus.Quests;

public readonly struct QuestEvent
{
    public string Key { get; }
    public int Value { get; }

    public QuestEvent(string key, int value = 1)
    {
        Key = key;
        Value = value;
    }
}
