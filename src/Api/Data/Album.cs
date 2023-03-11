using AutoBackend.Sdk.Attributes;

namespace Api.Data;

[GenericEntity(
    nameof(Id)
)]
[GenericController]
public class Album
{
    [GenericFilter] public Guid Id { get; set; }

    [GenericFilter] public string Title { get; set; } = null!;

    [GenericFilter] public string? Artist { get; set; }

    [GenericFilter] public int Score { get; set; } = 0;
}