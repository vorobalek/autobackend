using System.Collections.Concurrent;
using System.Linq.Expressions;
using AutoBackend.Sdk.Exceptions.Reflection;

namespace AutoBackend.Sdk.Data.Mappers;

internal class MapperExpressionsCache : IMapperExpressionsCache
{
    private readonly ConcurrentDictionary<ValueTuple<Type, Type>, Expression> _cacheExpressions = new();
    private readonly ConcurrentDictionary<ValueTuple<Type, Type>, object> _cacheFunc = new();
    
    public Func<TSource, TDestination> GetOrAddAndCompile<
        TSource,
        TDestination
    >(Expression<Func<TSource, TDestination>> newValue)
    {
        return _cacheFunc
                   .GetOrAdd(
                       (typeof(TSource), typeof(TDestination)),
                       _ => newValue.Compile()) as Func<TSource, TDestination>
               ?? throw new InheritanceReflectionException();
    }
}