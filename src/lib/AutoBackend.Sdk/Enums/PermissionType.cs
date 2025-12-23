namespace AutoBackend.Sdk.Enums;

[Flags]
internal enum PermissionType
{
    Create = 1,
    Read = 1 << 1,
    Update = 1 << 2,
    Delete = 1 << 3
}