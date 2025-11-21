namespace Model.Results;

public sealed record Result<T>(bool IsSuccess, T? Value, IDictionary<string, string[]>? Errors)
{
    public static Result<T> Success(T value) => new(true, value, null);

    public static Result<T> Failure(IDictionary<string, string[]> errors) => new(false, default, errors);
}