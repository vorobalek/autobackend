using System.ComponentModel.DataAnnotations.Schema;
using AutoBackend.Sdk.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Sample.Data;

[GenericEntity(
    nameof(UserId),
    nameof(BudgetId)
)]
[GenericController]
[GenericGqlQuery]
[GenericGqlMutation]
[GenericRequest(
    nameof(UserId),
    nameof(BudgetId)
)]
[GenericResponse(
    nameof(UserId),
    nameof(BudgetId)
)]
[GenericCreatePermission]
[GenericReadPermission]
[GenericUpdatePermission]
[GenericDeletePermission]
public class Participating
{
    [GenericFilter]
    [ForeignKey(nameof(User))]
    public long UserId { get; set; }

    [DeleteBehavior(DeleteBehavior.Cascade)]
    [InverseProperty(nameof(Data.User.Participating))]
    public User User { get; set; } = null!;

    [GenericFilter]
    [ForeignKey(nameof(Budget))]
    public Guid BudgetId { get; set; }

    [DeleteBehavior(DeleteBehavior.Cascade)]
    [InverseProperty(nameof(Data.Budget.Participating))]
    public Budget Budget { get; set; } = null!;
}