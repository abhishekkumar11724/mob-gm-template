namespace Nimbus.Core;

public static class Services
{
    private static readonly ServiceLocator _instance = new();

    public static void Register<T>(T service) => _instance.Register(service);

    public static void Unregister<T>() => _instance.Unregister<T>();

    public static T Get<T>() => _instance.Get<T>();

    public static bool TryGet<T>(out T service) => _instance.TryGet(out service);
}
