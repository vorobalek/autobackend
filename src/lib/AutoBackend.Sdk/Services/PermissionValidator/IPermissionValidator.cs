using AutoBackend.Sdk.Enums;

namespace AutoBackend.Sdk.Services.PermissionValidator;

internal interface IPermissionValidator
{
    void Validate<TEntity>(PermissionType type, HashSet<string>? affectedProperties = null) where TEntity : class, new();
}