﻿namespace Domain.Shared;

public class Error(string message)
{
    public string Message { get; } = message;

    public static Error None => new(string.Empty);

    public static implicit operator Error(string message) => new(message);

    public static implicit operator string(Error error) => error.Message;
}