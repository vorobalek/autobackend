using System.ComponentModel.DataAnnotations.Schema;
using AutoBackend.Sdk.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

[GenericEntity(
    nameof(AlbumId),
    nameof(SongId)
)]
[GenericController]
public class AlbumContent
{
    [GenericFilter]
    [ForeignKey(nameof(Album))]
    public Guid AlbumId { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(Song))]
    public Guid SongId { get; set; }

    [DeleteBehavior(DeleteBehavior.Cascade)]
    public Album Album { get; set; } = null!;

    [DeleteBehavior(DeleteBehavior.Cascade)]
    public Song Song { get; set; } = null!;
}