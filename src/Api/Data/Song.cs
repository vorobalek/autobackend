using AutoBackend.Sdk.Attributes;

namespace Api.Data;

[GenericEntity(
    nameof(Id)
)]
[GenericController]
[GenericGqlQuery]
[GenericGqlMutation]
public class Song
{
    [GenericFilter] public Guid Id { get; set; }

    [GenericFilter] public string Title { get; set; } = null!;

    [GenericFilter] public string? Author { get; set; }

    [GenericFilter] public string? Text { get; set; }

    [GenericFilter] public decimal Price { get; set; }
}