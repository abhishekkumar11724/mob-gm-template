namespace Nimbus.Core;

public interface ITickerService : IService
{
    void Add(ITickable tickable);

    void Remove(ITickable tickable);

    void Add(ILateTickable tickable);

    void Remove(ILateTickable tickable);
}
