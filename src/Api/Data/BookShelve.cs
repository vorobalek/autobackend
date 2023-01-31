using System.ComponentModel.DataAnnotations.Schema;
using AutoBackend.Sdk.Attributes.GenericControllers.V1;
using AutoBackend.Sdk.Attributes.GenericEntities;
using AutoBackend.Sdk.Attributes.GenericFilters;

namespace Api.Data;

[GenericEntity(
    nameof(Book1Id),
    nameof(Book2Id),
    nameof(Book3Id),
    nameof(Book4Id),
    nameof(Book5Id),
    nameof(Book6Id),
    nameof(Book7Id),
    nameof(Book8Id)
)]
[GenericControllerV1]
public class BookShelve
{
    [GenericFilter] public string? Name { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(Book1))]
    public Guid Book1Id { get; set; }

    public Book Book1 { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(Book2))]
    public Guid Book2Id { get; set; }

    public Book Book2 { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(Book3))]
    public Guid Book3Id { get; set; }

    public Book Book3 { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(Book4))]
    public Guid Book4Id { get; set; }

    public Book Book4 { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(Book5))]
    public Guid Book5Id { get; set; }

    public Book Book5 { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(Book6))]
    public Guid Book6Id { get; set; }

    public Book Book6 { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(Book7))]
    public Guid Book7Id { get; set; }

    public Book Book7 { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(Book8))]
    public Guid Book8Id { get; set; }

    public Book Book8 { get; set; }
}