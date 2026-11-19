using System.Collections.Generic;

namespace Nimbus.Notifications;

public sealed class NotificationData
{
    public string Id { get; }

    public string Title { get; }

    public string Body { get; }

    public IReadOnlyDictionary<string, string>? CustomData { get; }

    public NotificationData(string id, string title, string body, IReadOnlyDictionary<string, string>? customData = null)
    {
        Id = id;
        Title = title;
        Body = body;
        CustomData = customData;
    }
}
