namespace AutoBackend.Sdk.Services.EntityMetadataProvider;

internal interface IEntityMetadataProvider
{
    GenericEntityMetadata GetMetadata<TEntity>() where TEntity : class, new();
}