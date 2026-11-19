namespace Nimbus.Save;

public readonly struct SaveStarted
{
}

public readonly struct SaveCompleted
{
}

public readonly struct SaveFailed
{
    public readonly string Error;

    public SaveFailed(string error)
    {
        Error = error;
    }
}

public readonly struct LoadCompleted
{
}
