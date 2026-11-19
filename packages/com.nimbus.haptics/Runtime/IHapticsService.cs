using Nimbus.Core;

namespace Nimbus.Haptics;

public interface IHapticsService : IService
{
    bool IsEnabled { get; }

    void SetEnabled(bool enabled);

    void Play(HapticPattern pattern);
}
