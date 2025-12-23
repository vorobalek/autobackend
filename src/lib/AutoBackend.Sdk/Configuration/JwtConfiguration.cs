namespace AutoBackend.Sdk.Configuration;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed record JwtConfiguration
{
    public string PublicKey { get; init; } = null!;
    
    public string? ValidIssuer { get; init; }
    
    public string? ValidAudience { get; init; }
}