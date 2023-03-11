This is the public release of `v0.1.3` of the `AutoBackend.SDK`. This package provides the boilerplate infrastructure to
create simplified backend services by managing DataBase and API layers. For more information,
see [readme](https://github.com/vorobalek/autobackend/blob/main/README.md).

Developer builds are published on
the [specific MyGet developer feed](https://www.myget.org/feed/autobackend-dev/package/nuget/AutoBackend.SDK). Release
builds are published on [MyGet](https://www.myget.org/feed/autobackend/package/nuget/AutoBackend.SDK)
and [NuGet](https://www.nuget.org/packages/AutoBackend.SDK).

In this version:

- The only objects visible as public now are:
  - `GenericControllerAttribute`
  - `GenericEntityAttribute`
  - `GenericFilterAttribute`
  - `GenericDbContext` and inheritors (but it's not recommended to use them)
    - `InMemoryGenericDbContext`
    - `PostgresGenericDbContext`
    - `SqlServerGenericDbContext`
  - `AutoBackendException`
  - `AutoBackendHost`
- Inheritance from any library's publicly visible type has been restricted
- `AutoBackendDbContext` has been renamed to `GenericDbContext`
- `AutoBackendDbContext<TContext>` has been removed permanently
- `GenericControllerV1Attribute` has been removed permanently
- Initializing `AutoBackendException` outside the library has been restricted
- `GenericEntityAttribute.Keys` visibility has been changed to `internal`
- Refactoring
  - Namespaces have been simplified
  - Excess files have been removed
- Bug fixes
  - Wrong database provider determining has been fixed