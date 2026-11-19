namespace Nimbus.Core;

public interface IRandomService : IService
{
    int Next();

    int Next(int maxValue);

    int Next(int minValue, int maxValue);

    double NextDouble();

    void NextBytes(byte[] buffer);

    void SetSeed(int seed);
}
