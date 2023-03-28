using System.ComponentModel.DataAnnotations.Schema;
using AutoBackend.Sdk.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

[GenericEntity(
    nameof(Album1Id),
    nameof(Album2Id),
    nameof(Album3Id),
    nameof(Album4Id),
    nameof(Album5Id),
    nameof(Album6Id),
    nameof(Album7Id),
    nameof(Album8Id)
)]
[GenericController]
[GenericGqlQuery]
public class AlbumSet
{
    [GenericFilter]
    [ForeignKey(nameof(Album1))]
    public Guid Album1Id { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(Album2))]
    public Guid? Album2Id { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(Album3))]
    public Guid? Album3Id { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(Album4))]
    public Guid Album4Id { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(Album5))]
    public Guid? Album5Id { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(Album6))]
    public Guid? Album6Id { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(Album7))]
    public Guid? Album7Id { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(Album8))]
    public Guid? Album8Id { get; set; }

    [DeleteBehavior(DeleteBehavior.Cascade)]
    public Album Album1 { get; set; } = null!;

    [DeleteBehavior(DeleteBehavior.SetNull)]
    public Album? Album2 { get; set; }

    [DeleteBehavior(DeleteBehavior.SetNull)]
    public Song? Album3 { get; set; }

    [DeleteBehavior(DeleteBehavior.SetNull)]
    public Song? Album4 { get; set; }

    [DeleteBehavior(DeleteBehavior.SetNull)]
    public Song? Album5 { get; set; }

    [DeleteBehavior(DeleteBehavior.SetNull)]
    public Song? Album6 { get; set; }

    [DeleteBehavior(DeleteBehavior.SetNull)]
    public Song? Album7 { get; set; }

    [DeleteBehavior(DeleteBehavior.SetNull)]
    public Song? Album8 { get; set; }

    [GenericFilter] public string Name { get; set; } = null!;
}