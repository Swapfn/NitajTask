namespace Domain.Shared;
public class Result(bool isSuccess, List<Error>? errors = null)
{
    public bool IsSuccess { get; } = isSuccess;
    public List<Error>? Errors { get; } = errors;

    public static Result Success() => new(true, null);

    public static Result Error(List<Error> errors) => new(false, errors);

    public static Result<T> Success<T>(T data) => new(true, data, null);

    public static Result<T> Error<T>(List<Error> errors) => new(false, default, errors);
}

public class Result<T>(bool isSuccess, T? data, List<Error>? errors = null) : Result(isSuccess, errors)
{
    public T? Data { get; } = data;
}