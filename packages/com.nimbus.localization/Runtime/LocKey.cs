using System;

namespace Nimbus.Localization;

public readonly struct LocKey : IEquatable<LocKey>
{
    private readonly string _key;

    public LocKey(string key)
    {
        _key = key;
    }

    public string Key => _key ?? string.Empty;

    public bool IsEmpty => string.IsNullOrEmpty(_key);

    public bool Equals(LocKey other)
    {
        return string.Equals(Key, other.Key, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj)
    {
        return obj is LocKey other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Key.GetHashCode();
    }

    public override string ToString()
    {
        return Key;
    }

    public static bool operator ==(LocKey left, LocKey right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(LocKey left, LocKey right)
    {
        return !left.Equals(right);
    }

    public static implicit operator LocKey(string key)
    {
        return new LocKey(key);
    }
}
