using AutoBackend.Sdk.Attributes.GenericControllers.V1;
using AutoBackend.Sdk.Attributes.GenericEntities;
using AutoBackend.Sdk.Attributes.GenericFilters;

namespace Api.Data;

[GenericEntity(
    nameof(Id)
)]
[GenericControllerV1]
public class Book
{
    [GenericFilter] public Guid Id { get; set; }

    [GenericFilter] public string? Title { get; set; }

    public string? Author { get; set; }

    [GenericFilter] public decimal Price { get; set; }
}