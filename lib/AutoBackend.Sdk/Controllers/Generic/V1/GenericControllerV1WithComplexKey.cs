using AutoBackend.Sdk.Models.V1;
using AutoBackend.Sdk.Storage.Generic;
using Microsoft.AspNetCore.Mvc;

namespace AutoBackend.Sdk.Controllers.Generic.V1;

internal class GenericControllerV1WithComplexKey<
    TEntity,
    TFilter,
    TKey1,
    TKey2
> : GenericControllerV1<TEntity, TFilter>
    where TEntity : class
    where TFilter : class
{
    private readonly IGenericStorageWithComplexKey<
        TEntity,
        TKey1,
        TKey2
    > _genericStorage;

    public GenericControllerV1WithComplexKey(
        IGenericStorageWithComplexKey<
            TEntity,
            TKey1,
            TKey2
        > genericStorage) : base(genericStorage)
    {
        _genericStorage = genericStorage;
    }

    [HttpGet("{key1}/{key2}")]
    public Task<ActionResult<ApiResponseV1<TEntity?>>> GetByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2)
    {
        return ProcessAsync(cancellationToken => _genericStorage.GetByComplexKeyAsync(
            key1,
            key2,
            cancellationToken));
    }

    [HttpPost("{key1}/{key2}")]
    public Task<ActionResult<ApiResponseV1<TEntity>>> InsertByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericStorage.InsertByComplexKeyAsync(
            key1,
            key2,
            entity,
            cancellationToken));
    }

    [HttpPut("{key1}/{key2}")]
    public Task<ActionResult<ApiResponseV1<TEntity>>> UpdateByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericStorage.UpdateByComplexKeyAsync(
            key1,
            key2,
            entity,
            cancellationToken));
    }

    [HttpDelete("{key1}/{key2}")]
    public Task<ActionResult<ApiResponseV1>> DeleteByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2)
    {
        return ProcessAsync(cancellationToken => _genericStorage.DeleteByComplexKeyAsync(
            key1,
            key2,
            cancellationToken));
    }
}

internal class GenericControllerV1WithComplexKey<
    TEntity,
    TFilter,
    TKey1,
    TKey2,
    TKey3
> : GenericControllerV1<TEntity, TFilter>
    where TEntity : class
    where TFilter : class
{
    private readonly IGenericStorageWithComplexKey<
        TEntity,
        TKey1,
        TKey2,
        TKey3
    > _genericStorage;

    public GenericControllerV1WithComplexKey(
        IGenericStorageWithComplexKey<
            TEntity,
            TKey1,
            TKey2,
            TKey3
        > genericStorage) : base(genericStorage)
    {
        _genericStorage = genericStorage;
    }

    [HttpGet("{key1}/{key2}/{key3}")]
    public Task<ActionResult<ApiResponseV1<TEntity?>>> GetByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3)
    {
        return ProcessAsync(cancellationToken => _genericStorage.GetByComplexKeyAsync(
            key1,
            key2,
            key3,
            cancellationToken));
    }

    [HttpPost("{key1}/{key2}/{key3}")]
    public Task<ActionResult<ApiResponseV1<TEntity>>> InsertByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericStorage.InsertByComplexKeyAsync(
            key1,
            key2,
            key3,
            entity,
            cancellationToken));
    }

    [HttpPut("{key1}/{key2}/{key3}")]
    public Task<ActionResult<ApiResponseV1<TEntity>>> UpdateByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericStorage.UpdateByComplexKeyAsync(
            key1,
            key2,
            key3,
            entity,
            cancellationToken));
    }

    [HttpDelete("{key1}/{key2}/{key3}")]
    public Task<ActionResult<ApiResponseV1>> DeleteByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3)
    {
        return ProcessAsync(cancellationToken => _genericStorage.DeleteByComplexKeyAsync(
            key1,
            key2,
            key3,
            cancellationToken));
    }
}

internal class GenericControllerV1WithComplexKey<
    TEntity,
    TFilter,
    TKey1,
    TKey2,
    TKey3,
    TKey4
> : GenericControllerV1<TEntity, TFilter>
    where TEntity : class
    where TFilter : class
{
    private readonly IGenericStorageWithComplexKey<
        TEntity,
        TKey1,
        TKey2,
        TKey3,
        TKey4
    > _genericStorage;

    public GenericControllerV1WithComplexKey(
        IGenericStorageWithComplexKey<
            TEntity,
            TKey1,
            TKey2,
            TKey3,
            TKey4
        > genericStorage) : base(genericStorage)
    {
        _genericStorage = genericStorage;
    }

    [HttpGet("{key1}/{key2}/{key3}/{key4}")]
    public Task<ActionResult<ApiResponseV1<TEntity?>>> GetByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4)
    {
        return ProcessAsync(cancellationToken => _genericStorage.GetByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            cancellationToken));
    }

    [HttpPost("{key1}/{key2}/{key3}/{key4}")]
    public Task<ActionResult<ApiResponseV1<TEntity>>> InsertByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericStorage.InsertByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            entity,
            cancellationToken));
    }

    [HttpPut("{key1}/{key2}/{key3}/{key4}")]
    public Task<ActionResult<ApiResponseV1<TEntity>>> UpdateByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericStorage.UpdateByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            entity,
            cancellationToken));
    }

    [HttpDelete("{key1}/{key2}/{key3}/{key4}")]
    public Task<ActionResult<ApiResponseV1>> DeleteByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4)
    {
        return ProcessAsync(cancellationToken => _genericStorage.DeleteByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            cancellationToken));
    }
}

internal class GenericControllerV1WithComplexKey<
    TEntity,
    TFilter,
    TKey1,
    TKey2,
    TKey3,
    TKey4,
    TKey5
> : GenericControllerV1<TEntity, TFilter>
    where TEntity : class
    where TFilter : class
{
    private readonly IGenericStorageWithComplexKey<
        TEntity,
        TKey1,
        TKey2,
        TKey3,
        TKey4,
        TKey5
    > _genericStorage;

    public GenericControllerV1WithComplexKey(
        IGenericStorageWithComplexKey<
            TEntity,
            TKey1,
            TKey2,
            TKey3,
            TKey4,
            TKey5
        > genericStorage) : base(genericStorage)
    {
        _genericStorage = genericStorage;
    }

    [HttpGet("{key1}/{key2}/{key3}/{key4}/{key5}")]
    public Task<ActionResult<ApiResponseV1<TEntity?>>> GetByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5)
    {
        return ProcessAsync(cancellationToken => _genericStorage.GetByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            cancellationToken));
    }

    [HttpPost("{key1}/{key2}/{key3}/{key4}/{key5}")]
    public Task<ActionResult<ApiResponseV1<TEntity>>> InsertByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericStorage.InsertByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            entity,
            cancellationToken));
    }

    [HttpPut("{key1}/{key2}/{key3}/{key4}/{key5}")]
    public Task<ActionResult<ApiResponseV1<TEntity>>> UpdateByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericStorage.UpdateByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            entity,
            cancellationToken));
    }

    [HttpDelete("{key1}/{key2}/{key3}/{key4}/{key5}")]
    public Task<ActionResult<ApiResponseV1>> DeleteByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5)
    {
        return ProcessAsync(cancellationToken => _genericStorage.DeleteByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            cancellationToken));
    }
}

internal class GenericControllerV1WithComplexKey<
    TEntity,
    TFilter,
    TKey1,
    TKey2,
    TKey3,
    TKey4,
    TKey5,
    TKey6
> : GenericControllerV1<TEntity, TFilter>
    where TEntity : class
    where TFilter : class
{
    private readonly IGenericStorageWithComplexKey<
        TEntity,
        TKey1,
        TKey2,
        TKey3,
        TKey4,
        TKey5,
        TKey6
    > _genericStorage;

    public GenericControllerV1WithComplexKey(
        IGenericStorageWithComplexKey<
            TEntity,
            TKey1,
            TKey2,
            TKey3,
            TKey4,
            TKey5,
            TKey6
        > genericStorage) : base(genericStorage)
    {
        _genericStorage = genericStorage;
    }

    [HttpGet("{key1}/{key2}/{key3}/{key4}/{key5}/{key6}")]
    public Task<ActionResult<ApiResponseV1<TEntity?>>> GetByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromRoute] TKey6 key6)
    {
        return ProcessAsync(cancellationToken => _genericStorage.GetByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            key6,
            cancellationToken));
    }

    [HttpPost("{key1}/{key2}/{key3}/{key4}/{key5}/{key6}")]
    public Task<ActionResult<ApiResponseV1<TEntity>>> InsertByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromRoute] TKey6 key6,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericStorage.InsertByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            key6,
            entity,
            cancellationToken));
    }

    [HttpPut("{key1}/{key2}/{key3}/{key4}/{key5}/{key6}")]
    public Task<ActionResult<ApiResponseV1<TEntity>>> UpdateByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromRoute] TKey6 key6,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericStorage.UpdateByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            key6,
            entity,
            cancellationToken));
    }

    [HttpDelete("{key1}/{key2}/{key3}/{key4}/{key5}/{key6}")]
    public Task<ActionResult<ApiResponseV1>> DeleteByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromRoute] TKey6 key6)
    {
        return ProcessAsync(cancellationToken => _genericStorage.DeleteByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            key6,
            cancellationToken));
    }
}

internal class GenericControllerV1WithComplexKey<
    TEntity,
    TFilter,
    TKey1,
    TKey2,
    TKey3,
    TKey4,
    TKey5,
    TKey6,
    TKey7
> : GenericControllerV1<TEntity, TFilter>
    where TEntity : class
    where TFilter : class
{
    private readonly IGenericStorageWithComplexKey<
        TEntity,
        TKey1,
        TKey2,
        TKey3,
        TKey4,
        TKey5,
        TKey6,
        TKey7
    > _genericStorage;

    public GenericControllerV1WithComplexKey(
        IGenericStorageWithComplexKey<
            TEntity,
            TKey1,
            TKey2,
            TKey3,
            TKey4,
            TKey5,
            TKey6,
            TKey7
        > genericStorage) : base(genericStorage)
    {
        _genericStorage = genericStorage;
    }

    [HttpGet("{key1}/{key2}/{key3}/{key4}/{key5}/{key6}/{key7}")]
    public Task<ActionResult<ApiResponseV1<TEntity?>>> GetByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromRoute] TKey6 key6,
        [FromRoute] TKey7 key7)
    {
        return ProcessAsync(cancellationToken => _genericStorage.GetByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            key6,
            key7,
            cancellationToken));
    }

    [HttpPost("{key1}/{key2}/{key3}/{key4}/{key5}/{key6}/{key7}")]
    public Task<ActionResult<ApiResponseV1<TEntity>>> InsertByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromRoute] TKey6 key6,
        [FromRoute] TKey7 key7,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericStorage.InsertByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            key6,
            key7,
            entity,
            cancellationToken));
    }

    [HttpPut("{key1}/{key2}/{key3}/{key4}/{key5}/{key6}/{key7}")]
    public Task<ActionResult<ApiResponseV1<TEntity>>> UpdateByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromRoute] TKey6 key6,
        [FromRoute] TKey7 key7,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericStorage.UpdateByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            key6,
            key7,
            entity,
            cancellationToken));
    }

    [HttpDelete("{key1}/{key2}/{key3}/{key4}/{key5}/{key6}/{key7}")]
    public Task<ActionResult<ApiResponseV1>> DeleteByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromRoute] TKey6 key6,
        [FromRoute] TKey7 key7)
    {
        return ProcessAsync(cancellationToken => _genericStorage.DeleteByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            key6,
            key7,
            cancellationToken));
    }
}

internal class GenericControllerV1WithComplexKey<
    TEntity,
    TFilter,
    TKey1,
    TKey2,
    TKey3,
    TKey4,
    TKey5,
    TKey6,
    TKey7,
    TKey8
> : GenericControllerV1<TEntity, TFilter>
    where TEntity : class
    where TFilter : class
{
    private readonly IGenericStorageWithComplexKey<
        TEntity,
        TKey1,
        TKey2,
        TKey3,
        TKey4,
        TKey5,
        TKey6,
        TKey7,
        TKey8
    > _genericStorage;

    public GenericControllerV1WithComplexKey(
        IGenericStorageWithComplexKey<
            TEntity,
            TKey1,
            TKey2,
            TKey3,
            TKey4,
            TKey5,
            TKey6,
            TKey7,
            TKey8
        > genericStorage) : base(genericStorage)
    {
        _genericStorage = genericStorage;
    }

    [HttpGet("{key1}/{key2}/{key3}/{key4}/{key5}/{key6}/{key7}/{key8}")]
    public Task<ActionResult<ApiResponseV1<TEntity?>>> GetByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromRoute] TKey6 key6,
        [FromRoute] TKey7 key7,
        [FromRoute] TKey8 key8)
    {
        return ProcessAsync(cancellationToken => _genericStorage.GetByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            key6,
            key7,
            key8,
            cancellationToken));
    }

    [HttpPost("{key1}/{key2}/{key3}/{key4}/{key5}/{key6}/{key7}/{key8}")]
    public Task<ActionResult<ApiResponseV1<TEntity>>> InsertByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromRoute] TKey6 key6,
        [FromRoute] TKey7 key7,
        [FromRoute] TKey8 key8,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericStorage.InsertByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            key6,
            key7,
            key8,
            entity,
            cancellationToken));
    }

    [HttpPut("{key1}/{key2}/{key3}/{key4}/{key5}/{key6}/{key7}/{key8}")]
    public Task<ActionResult<ApiResponseV1<TEntity>>> UpdateByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromRoute] TKey6 key6,
        [FromRoute] TKey7 key7,
        [FromRoute] TKey8 key8,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericStorage.UpdateByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            key6,
            key7,
            key8,
            entity,
            cancellationToken));
    }

    [HttpDelete("{key1}/{key2}/{key3}/{key4}/{key5}/{key6}/{key7}/{key8}")]
    public Task<ActionResult<ApiResponseV1>> DeleteByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromRoute] TKey6 key6,
        [FromRoute] TKey7 key7,
        [FromRoute] TKey8 key8)
    {
        return ProcessAsync(cancellationToken => _genericStorage.DeleteByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            key6,
            key7,
            key8,
            cancellationToken));
    }
}