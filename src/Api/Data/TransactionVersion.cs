using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoBackend.Sdk.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

[GenericEntity]
[GenericController]
[GenericRequest(
    nameof(TransactionId),
    nameof(UserId),
    nameof(BudgetId),
    nameof(Amount),
    nameof(DateTimeUtc),
    nameof(Comment),
    nameof(OriginalTransactionId),
    nameof(VersionDateTimeUtc)
)]
[GenericResponse(
    nameof(TransactionId),
    nameof(UserId),
    nameof(BudgetId),
    nameof(Amount),
    nameof(DateTimeUtc),
    nameof(Comment),
    nameof(OriginalTransactionId),
    nameof(VersionDateTimeUtc)
)]
[GenericGqlQuery]
[GenericGqlMutation]
public class TransactionVersion
{
    [GenericFilter]
    [ForeignKey(nameof(Transaction))]
    public Guid TransactionId { get; set; }

    [InverseProperty(nameof(Data.Transaction.Versions))]
    public Transaction Transaction { get; set; } = null!;
    
    [GenericFilter]
    public Guid OriginalTransactionId { get; set; }

    [GenericFilter]
    public DateTime VersionDateTimeUtc { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(User))]
    public long? UserId { get; set; }

    [DeleteBehavior(DeleteBehavior.SetNull)]
    public User? User { get; set; }

    [GenericFilter]
    [ForeignKey(nameof(Budget))]
    public Guid BudgetId { get; set; }

    [DeleteBehavior(DeleteBehavior.Cascade)]
    public Budget Budget { get; set; } = null!;

    [GenericFilter]
    [Precision(20,4)]
    public decimal Amount { get; set; }

    [GenericFilter]
    public DateTime DateTimeUtc { get; set; }

    [GenericFilter]
    [MaxLength(250)]
    public string Comment { get; set; } = null!;
}