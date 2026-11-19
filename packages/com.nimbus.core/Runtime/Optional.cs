using System;
using System.Collections.Generic;

namespace Nimbus.Core;

public readonly struct Optional<T> : IEquatable<Optional<T>>
{
    private readonly T _value;
    private readonly bool _hasValue;

    public Optional(T value)
    {
        _value = value;
        _hasValue = true;
    }

    public static Optional<T> None => new();

    public bool HasValue => _hasValue;

    public T Value
    {
        get
        {
            if (!_hasValue)
            {
                throw new InvalidOperationException("Optional has no value.");
            }

            return _value;
        }
    }

    public T GetValueOrDefault(T defaultValue = default!)
    {
        return _hasValue ? _value : defaultValue;
    }

    public bool TryGetValue(out T value)
    {
        value = _value;
        return _hasValue;
    }

    public static implicit operator Optional<T>(T value)
    {
        return value != null ? new Optional<T>(value) : None;
    }

    public bool Equals(Optional<T> other)
    {
        if (!_hasValue && !other._hasValue)
        {
            return true;
        }

        if (_hasValue != other._hasValue)
        {
            return false;
        }

        return EqualityComparer<T>.Default.Equals(_value, other._value);
    }

    public override bool Equals(object? obj)
    {
        return obj is Optional<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_value, _hasValue);
    }

    public static bool operator ==(Optional<T> left, Optional<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Optional<T> left, Optional<T> right)
    {
        return !left.Equals(right);
    }
}
