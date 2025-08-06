using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoBackend.Sdk.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Sample.Data;

[GenericEntity(
    nameof(Id)
)]
[GenericController]
[GenericGqlQuery]
[GenericGqlMutation]
[GenericRequest(
    nameof(Id),
    nameof(UserId),
    nameof(BudgetId),
    nameof(Amount),
    nameof(DateTimeUtc),
    nameof(Comment),
    nameof(SecretKey)
)]
[GenericResponse(
    nameof(Id),
    nameof(UserId),
    nameof(BudgetId),
    nameof(Amount),
    nameof(DateTimeUtc),
    nameof(Comment)
)]
public class Transaction
{
    [GenericFilter]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(User))]
    public long? UserId { get; set; }

    [DeleteBehavior(DeleteBehavior.SetNull)]
    [InverseProperty(nameof(Data.User.Transactions))]
    public User? User { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(Budget))]
    public Guid BudgetId { get; set; }

    [DeleteBehavior(DeleteBehavior.Cascade)]
    [InverseProperty(nameof(Data.Budget.Transactions))]
    public Budget Budget { get; set; } = null!;

    [GenericFilter]
    [Precision(20, 4)]
    public decimal Amount { get; set; }

    [GenericFilter]
    public DateTime DateTimeUtc { get; set; }

    [GenericFilter]
    [MaxLength(250)]
    public string Comment { get; set; } = null!;

    [MaxLength(250)]
    public string SecretKey { get; set; } = null!;

    [InverseProperty(nameof(TransactionVersion.Transaction))]
    public ICollection<TransactionVersion> Versions { get; set; } = new List<TransactionVersion>();
}