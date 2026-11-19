namespace Nimbus.Core.Events;

public readonly struct AppReady
{
}

public readonly struct ApplicationPaused
{
}

public readonly struct ApplicationResumed
{
}

public readonly struct ServiceRegistered
{
    public readonly object Service;

    public ServiceRegistered(object service)
    {
        Service = service;
    }
}
