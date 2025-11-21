namespace Model.Services;

public interface IDateTimeProvider
{
    DateTime GetUtcNow();
}