using System.Linq.Expressions;

namespace AutoBackend.Sdk.Data.Mappers;

internal interface IMapperExpressionsCache
{
    Func<TSource, TDestination> GetOrAddAndCompile<
        TSource,
        TDestination
    >(Expression<Func<TSource, TDestination>> newValue);
}