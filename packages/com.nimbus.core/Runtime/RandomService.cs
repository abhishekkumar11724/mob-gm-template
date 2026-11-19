using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Nimbus.Core;

public sealed class RandomService : IRandomService, IInitializable
{
    private Random _random = new();

    public int Next() => _random.Next();

    public int Next(int maxValue) => _random.Next(maxValue);

    public int Next(int minValue, int maxValue) => _random.Next(minValue, maxValue);

    public double NextDouble() => _random.NextDouble();

    public void NextBytes(byte[] buffer) => _random.NextBytes(buffer);

    public void SetSeed(int seed)
    {
        _random = new Random(seed);
    }

    public UniTask InitializeAsync(CancellationToken cancellationToken)
    {
        return UniTask.CompletedTask;
    }
}
