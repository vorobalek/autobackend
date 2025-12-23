using System.Reflection;
using AutoBackend.Sdk.Attributes;
using AutoBackend.Sdk.Enums;

namespace AutoBackend.Sdk.Services.EntityMetadataProvider;

internal sealed class EntityMetadataProvider : IEntityMetadataProvider
{
    private readonly Dictionary<Type, GenericEntityMetadata> _cache = new();
    
    public GenericEntityMetadata GetMetadata<TEntity>() where TEntity : class, new()
    {
        var type = typeof(TEntity);
        if (_cache.TryGetValue(type, out var metadata)) return metadata;
        metadata = CreateMetadata(type);
        _cache.Add(type, metadata);
        return metadata;
    }

    private GenericEntityMetadata CreateMetadata(Type type)
    {
        var propertyInfos = type.GetProperties();
        var properties = propertyInfos.Select(CreateMetadata).ToList();
        var permissions = CreatePermissionType(type);
        return new GenericEntityMetadata(
            type.Name,
            permissions,
            properties);
    }

    private GenericPropertyMetadata CreateMetadata(PropertyInfo propertyInfo)
    {
        var permissions = CreatePermissionType(propertyInfo);
        return new GenericPropertyMetadata(
            propertyInfo.Name,
            permissions);
    }

    private static PermissionType CreatePermissionType(Type type)
    {
        PermissionType permissionType = 0;
        if (type.GetCustomAttribute<GenericCreatePermissionAttribute>() is not null)
        {
            permissionType |= PermissionType.Create;
        }
        if (type.GetCustomAttribute<GenericReadPermissionAttribute>() is not null)
        {
            permissionType |= PermissionType.Read;
        }
        if (type.GetCustomAttribute<GenericUpdatePermissionAttribute>() is not null)
        {
            permissionType |= PermissionType.Update;
        }
        if (type.GetCustomAttribute<GenericDeletePermissionAttribute>() is not null)
        {
            permissionType |= PermissionType.Delete;
        }
        return permissionType;
    }

    private static PermissionType CreatePermissionType(PropertyInfo propertyInfo)
    {
        PermissionType permissionType = 0;
        if (propertyInfo.GetCustomAttribute<GenericCreatePermissionAttribute>() is not null)
        {
            permissionType |= PermissionType.Create;
        }
        if (propertyInfo.GetCustomAttribute<GenericReadPermissionAttribute>() is not null)
        {
            permissionType |= PermissionType.Read;
        }
        if (propertyInfo.GetCustomAttribute<GenericUpdatePermissionAttribute>() is not null)
        {
            permissionType |= PermissionType.Update;
        }
        if (propertyInfo.GetCustomAttribute<GenericDeletePermissionAttribute>() is not null)
        {
            permissionType |= PermissionType.Delete;
        }
        return permissionType;
    }
}