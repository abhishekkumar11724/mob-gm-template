using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Nimbus.Core;

public sealed class TimeService : ITimeService, IInitializable
{
    private TimeSpan _serverTimeOffset;

    public float ScaledTime => Time.time;

    public float UnscaledTime => Time.unscaledTime;

    public float ScaledDelta => Time.deltaTime;

    public float UnscaledDelta => Time.unscaledDeltaTime;

    public DateTimeOffset ServerTime => DateTimeOffset.UtcNow.Add(_serverTimeOffset);

    public UniTask InitializeAsync(CancellationToken cancellationToken)
    {
        return UniTask.CompletedTask;
    }

    public void SetServerTime(DateTimeOffset serverTime)
    {
        _serverTimeOffset = serverTime - DateTimeOffset.UtcNow;
    }
}
