namespace Nimbus.Notifications;

public sealed class NotificationOptions
{
    public string? ChannelId { get; set; }

    public string? SmallIcon { get; set; }

    public string? LargeIcon { get; set; }

    public bool Repeat { get; set; }

    public int? BadgeNumber { get; set; }
}
