namespace AutoBackend.Sdk.Services.DateTimeProvider;

internal class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset Now()
    {
        return DateTimeOffset.Now;
    }

    public DateTimeOffset UtcNow()
    {
        return DateTimeOffset.UtcNow;
    }
}