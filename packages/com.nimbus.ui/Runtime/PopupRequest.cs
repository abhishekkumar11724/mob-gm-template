namespace Nimbus.UI;

public struct PopupRequest
{
    public PopupPriority Priority;
    public object? Data;

    public PopupRequest(PopupPriority priority = PopupPriority.Normal, object? data = null)
    {
        Priority = priority;
        Data = data;
    }
}
