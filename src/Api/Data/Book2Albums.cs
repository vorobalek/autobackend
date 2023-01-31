using System.ComponentModel.DataAnnotations.Schema;
using AutoBackend.Sdk.Attributes.GenericControllers.V1;
using AutoBackend.Sdk.Attributes.GenericEntities;
using AutoBackend.Sdk.Attributes.GenericFilters;

namespace Api.Data;

[GenericEntity(
    nameof(BookId),
    nameof(AlbumId)
)]
[GenericControllerV1]
public class Book2Albums
{
    [ForeignKey(nameof(Book))]
    [GenericFilter]
    public Guid BookId { get; set; }

    public Book Book { get; set; }

    [ForeignKey(nameof(Album))]
    [GenericFilter]
    public Guid AlbumId { get; set; }

    public Album Album { get; set; }
}