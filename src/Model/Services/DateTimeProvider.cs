namespace Model.Services;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetUtcNow()
    {
        return DateTime.UtcNow;
    }
}