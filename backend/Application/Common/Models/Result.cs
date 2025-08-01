﻿using CleanArch.Domain.Common;

namespace CleanArch.Application.Common.Models;

public interface IOperationResult
{
    bool IsSuccess { get; }
    bool IsFailure { get; }
    Error Error { get; }

    static abstract IOperationResult CreateFailure(Error error);
}

public class Result : IOperationResult
{
    public Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    public static IOperationResult CreateFailure(Error error) => Failure(error);
}

public class Result<TValue>(TValue? value, bool isSuccess, Error error)
    : Result(isSuccess, error),
        IOperationResult
{
    public TValue Value =>
        IsSuccess
            ? value!
            : throw new InvalidOperationException(
                "The value of a failure result can't be accessed."
            );

    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

    public static new IOperationResult CreateFailure(Error error) => Failure<TValue>(error);
}
