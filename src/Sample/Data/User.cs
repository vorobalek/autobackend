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
    nameof(FirstName),
    nameof(LastName),
    nameof(TimeZone),
    nameof(ActiveBudgetId)
)]
[GenericResponse(
    nameof(Id),
    nameof(FirstName),
    nameof(LastName),
    nameof(TimeZone),
    nameof(ActiveBudgetId)
)]
[GenericGqlQuery]
[GenericGqlMutation]
public class User
{
    [GenericFilter]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    [GenericFilter]
    public TimeSpan? TimeZone { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(ActiveBudget))]
    public Guid? ActiveBudgetId { get; set; }

    [DeleteBehavior(DeleteBehavior.SetNull)]
    [InverseProperty(nameof(Budget.ActiveUsers))]
    public Budget? ActiveBudget { get; set; }

    [InverseProperty(nameof(Budget.Owner))]
    public ICollection<Budget> OwnedBudgets { get; set; } = new List<Budget>();

    [InverseProperty(nameof(Data.Participating.User))]
    public ICollection<Participating> Participating { get; set; } = new List<Participating>();

    [InverseProperty(nameof(Transaction.User))]
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}