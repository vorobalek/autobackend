using AutoBackend.Sdk.Attributes.GenericControllers.V1;
using AutoBackend.Sdk.Attributes.GenericEntities;
using AutoBackend.Sdk.Attributes.GenericFilters;

namespace Api.Data;

[GenericEntity(
    nameof(Id)
)]
[GenericControllerV1]
public class Album
{
    [GenericFilter] public Guid Id { get; set; }

    public string? Title { get; set; }

    [GenericFilter] public string? Artist { get; set; }

    [GenericFilter] public int Score { get; set; } = 0;
}