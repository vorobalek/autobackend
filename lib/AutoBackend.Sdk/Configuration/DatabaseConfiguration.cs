using AutoBackend.Sdk.Enums;

namespace AutoBackend.Sdk.Configuration;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed record DatabaseConfiguration
{
    public DatabaseProviderType PrimaryProvider { get; init; }

    public Dictionary<DatabaseProviderType, string> Providers { get; init; } = null!;
}