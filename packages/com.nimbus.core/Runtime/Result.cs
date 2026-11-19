using System;

namespace Nimbus.Core;

public readonly struct Result<T>
{
    private readonly T _value;
    private readonly string _error;
    private readonly bool _isSuccess;

    private Result(T value)
    {
        _value = value;
        _error = string.Empty;
        _isSuccess = true;
    }

    private Result(string error)
    {
        _value = default!;
        _error = error;
        _isSuccess = false;
    }

    public bool IsSuccess => _isSuccess;

    public bool IsFailure => !_isSuccess;

    public T Value
    {
        get
        {
            if (!_isSuccess)
            {
                throw new InvalidOperationException($"Result is failure: {_error}");
            }

            return _value;
        }
    }

    public string Error => _error;

    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }

    public static Result<T> Failure(string error)
    {
        if (string.IsNullOrEmpty(error))
        {
            throw new ArgumentException("Error message cannot be empty.", nameof(error));
        }

        return new Result<T>(error);
    }

    public static implicit operator Result<T>(T value)
    {
        return Success(value);
    }

    public T GetValueOrDefault(T defaultValue = default!)
    {
        return _isSuccess ? _value : defaultValue;
    }

    public bool TryGetValue(out T value)
    {
        value = _value;
        return _isSuccess;
    }
}
