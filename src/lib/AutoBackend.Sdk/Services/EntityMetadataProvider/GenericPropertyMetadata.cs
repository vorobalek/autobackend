using AutoBackend.Sdk.Enums;

namespace AutoBackend.Sdk.Services.EntityMetadataProvider;

internal record GenericPropertyMetadata(
    string Name,
    PermissionType Permissions);