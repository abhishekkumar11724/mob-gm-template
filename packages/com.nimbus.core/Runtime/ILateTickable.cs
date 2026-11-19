namespace Nimbus.Core;

public interface ILateTickable
{
    void Tick(float deltaTime);
}
