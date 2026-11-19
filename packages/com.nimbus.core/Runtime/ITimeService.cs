using System;

namespace Nimbus.Core;

public interface ITimeService : IService
{
    float ScaledTime { get; }

    float UnscaledTime { get; }

    float ScaledDelta { get; }

    float UnscaledDelta { get; }

    DateTimeOffset ServerTime { get; }

    void SetServerTime(DateTimeOffset serverTime);
}
