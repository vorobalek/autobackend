using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoBackend.Sdk.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Sample.Data;

[GenericEntity(
    nameof(Id)
)]
[GenericController]
[GenericRequest(
    nameof(Id),
    nameof(Name),
    nameof(OwnerId)
)]
[GenericResponse(
    nameof(Id),
    nameof(Name),
    nameof(OwnerId)
)]
[GenericGqlQuery]
[GenericGqlMutation]
[Index(
    nameof(Name),
    nameof(OwnerId)
)]
public class Budget
{
    [GenericFilter]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [GenericFilter]
    [MaxLength(250)]
    public string Name { get; set; } = null!;

    [GenericFilter]
    [ForeignKey(nameof(Owner))]
    public long? OwnerId { get; set; }

    [DeleteBehavior(DeleteBehavior.SetNull)]
    [InverseProperty(nameof(User.OwnedBudgets))]
    public User? Owner { get; set; }

    [InverseProperty(nameof(User.ActiveBudget))]
    public ICollection<User> ActiveUsers { get; set; } = new List<User>();

    [InverseProperty(nameof(Transaction.Budget))]
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    [InverseProperty(nameof(Data.Participating.Budget))]
    public ICollection<Participating> Participating { get; set; } = new List<Participating>();
}