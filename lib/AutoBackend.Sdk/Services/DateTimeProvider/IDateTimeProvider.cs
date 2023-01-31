namespace AutoBackend.Sdk.Services.DateTimeProvider;

internal interface IDateTimeProvider
{
    DateTimeOffset Now();
    DateTimeOffset UtcNow();
}