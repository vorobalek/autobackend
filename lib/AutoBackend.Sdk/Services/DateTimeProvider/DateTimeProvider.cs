namespace AutoBackend.Sdk.Services.DateTimeProvider;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow()
    {
        return DateTimeOffset.UtcNow;
    }
}