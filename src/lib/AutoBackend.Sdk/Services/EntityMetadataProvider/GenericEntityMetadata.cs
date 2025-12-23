using AutoBackend.Sdk.Enums;

namespace AutoBackend.Sdk.Services.EntityMetadataProvider;

internal record GenericEntityMetadata(
    string Name,
    PermissionType Permissions,
    IEnumerable<GenericPropertyMetadata> Properties);