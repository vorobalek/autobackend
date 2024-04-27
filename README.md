# AutoBackend

| DEVELOPER BUILDS                                                                                                                                                                     | RELEASE BUILDS                                                                                                                                                                                                                                                                                                          |
|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [![MyGet](https://img.shields.io/myget/autobackend-dev/v/AutoBackend.SDK?label=myget&style=for-the-badge)](https://www.myget.org/feed/autobackend-dev/package/nuget/AutoBackend.SDK) | [![MyGet](https://img.shields.io/myget/autobackend/v/AutoBackend.SDK?label=myget&style=for-the-badge)](https://www.myget.org/feed/autobackend/package/nuget/AutoBackend.SDK) [![NuGet](https://img.shields.io/nuget/v/AutoBackend.SDK?label=nuget&style=for-the-badge)](https://www.nuget.org/packages/AutoBackend.SDK) |

This package provides the boilerplate infrastructure to create simplified backend services by managing DataBase & API
layers. It is a pet project without a commercial base and any promises about further development. If you would like to
use this package or sources of this package, please let me know by texting me@vorobalek.dev. I would prefer if any of
your scenarios of using this package left consequences in the form of contribution to this repository.

# Features

- [Database](#database)
    - [Schema modeling](#schema-modeling)
        - [Keyless entities](#keyless-entities)
        - [Single-PK entities](#single-pk-entities)
        - [Multi-PK entities](#multi-pk-entities)
    - [Providers](#providers)
    - [Migrations](#migrations)
        - [Migrate on startup](#migrate-on-startup)
- [API](#api)
    - [HTTP API](#http-api)
    - [GraphQL](#graphql)
        - [Queries](#queries)
        - [Mutations](#mutations)
    - [Modeling](#modeling)
    - [Filtering](#filtering)
    - _Authorization: isn't supported yet (might be expected in further releases)_

# Initialization

The sample using scenario can be found in [here](src/Sample).
Also, here are the copies of that samples.

## Initialize AutoBackend from your Program.cs file

```csharp
using AutoBackend.Sdk;

await new AutoBackendHost<Program>().RunAsync(args);
```

## Create the models you need to describe your domain relations

This example references to the [sample project models](src/Sample/Data).

```csharp
public class Budget
{
    public Guid Id { get; set; }
    
    // ..
}

public class Transaction
{
    public Guid Id { get; set; }
    
    // ..
}

public class TransactionVersion
{
    // ..
}

public class User
{
    public long Id { get; set; }
    
    // ..
}

public class Participating
{
    public long UserId { get; set; }
    
    public Guid BudgetId { get; set; }
    
    // ..
}
```

## See bellow to setup all features

---

# Database

Currently, only two types of relational database are supported (plus in-memory):

- SqlServer
- Postgres

AutoBackend.SDK will create tables and relations between them for entities you have configured.

## Schema modeling

As we all know there are three types of database tables:

- without primary key,
- with single-column primary key,
- with multi-column primary key.

Depends on how many columns your entity has, follow the instruction bellow to make AutoBackend.SDK able to observe its
changes.

### Keyless entities

Use `[GenericEntity]` to mark the model as a keyless entity.

```csharp
[GenericEntity]
public class TransactionVersion
{
    // ...
}
```

### Single-PK entities

Use `[GenericEntity(<primary key property name>)]` to mark the model as an entity with the primary key displayed as a
single property.

```csharp
[GenericEntity(nameof(Id))]
public class Budget
{
    public Guid Id { get; set; }

    // ...
}
```

### Multi-PK entities

Use `[GenericEntity(<first complex primary key property name>, <second complex primary key property name>, ...,  <N-th complex primary key>)]`
to mark the model as an entity with the primary key displayed as a complex set of properties.

> The maximum count of key properties in the complex key is _currently_ **8**.
>
> I hope, it's enough for each scenario you are considering about.

```csharp
[GenericEntity(nameof(UserId), nameof(BudgetId))]
public class Participating
{
    public long UserId { get; set; }

    public Guid BudgetId { get; set; }
    
    // ...
}
```

## Providers

To manage database connections, choose the "Database" configuration section.

```json
"Database": {
  "PrimaryProvider": "InMemory",
  "Providers": {
    "InMemory": "InMemory database name",
    "SqlServer": "SqlServer database connection string",
    "Postgres": "Postgres database connection string"
  }
}
```

The property `PrimaryProvider` allows one of the following string values:

- `InMemory`,
- `SqlServer`,
- `Postgres`.

The property `Providers` shall contain an object with optional properties: `InMemory`,  `SqlServer`, or `Postgres`. If
you chose the `PrimaryProvider` value, the property with the same name as that value is required.

## Migrations

First, you need to [install Entity Framework Core Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet).
After that, you can create a new migration using one of the following commands executed from the root of the project
folder.

`dotnet ef migrations add "<your migration name>" -o Migrations/SqlServer -c SqlServerGenericDbContext` - if you use
SqlServer.

`dotnet ef migrations add "<your migration name>" -o Migrations/Postgres -c PostgresGenericDbContext` - if you use
Postgres.

Or you can create scripts (like I did)
for [adding a new migration](add_migration.sh)
or [removing the last migration](remove_migration.sh) for both
database providers.

Finally, suppose you did not choose to delegate the database migrating to AutoBackend (see above). In that case, you can
migrate it yourself, executing `dotnet ef database update` from the root of the project folder.

### Migrate on startup

Suppose you use a relational database (like SqlServer or Postgres). In that case, you can let AutoBackend know whether
it has to migrate the database automatically on the application startup or doesn't, passing
the `migrateRelationalOnStartup` parameter to the `RunAsync` method in the place you call it to initialize AutoBackend.
Here is an example:

```csharp
await new AutoBackend.Sdk.AutoBackendHost<Program>().RunAsync(args, migrateRelationalOnStartup: true);
```

# API

Currently, only two types of AutoBackend.SDK-based application interfaces are supported:

- Old-school HTTP API
- Dummy GraphQL

## HTTP API

Mark with `[GenericController]` attribute the models which AutoBackend has to generate HTTP API endpoints for

> ❗**Disclaimer**
>
> Be noticed that `[GenericController]` only supports models which also marked with `[GenericEntity]`

### Response container and endpoints

The latest and the only API version now is v1. API v1 supports JSON only, and its output uses response container:

```
{
  "ok": boolean,
  "error_code": number,
  "description": string,
  "request_time_ms": number,
  "result": <response object>
}
```

For more details, you can always request `/swagger` to get the information about all generated endpoints.

### Endpoints

`[GenericController]` attribute will generate the following HTTP API endpoints:

| Method   | Url                                          | Keyless | Single-PK | Multi-PK | Description                                                                |
|----------|----------------------------------------------|---------|-----------|----------|----------------------------------------------------------------------------|
| `GET`    | `/api/v1/<model name>`                       | 🟢      | 🟢        | 🟢       | Get **all** entities of a specific model                                   |
| `GET`    | `/api/v1/<model name>/count`                 | 🟢      | 🟢        | 🟢       | Get **count** of entities of a specific model                              |
| `GET`    | `/api/v1/<model name>/<pk1>`                 | 🔴      | 🟢        | 🔴       | Get an entity with the requested primary key of a specific model           |
| `GET`    | `/api/v1/<model name>/<pk1>/<pk2>/.../<pkN>` | 🔴      | 🔴        | 🟢       | Get an entity with the requested primary key of a specific model           |
| `POST`   | `/api/v1/<model name>/<pk1>`                 | 🔴      | 🟢        | 🔴       | Create a new one entity with the specified primary key of a specific model |
| `POST`   | `/api/v1/<model name>/<pk1>/<pk2>/.../<pkN>` | 🔴      | 🔴        | 🟢       | Create a new one entity with the specified primary key of a specific model |
| `PUT`    | `/api/v1/<model name>/<pk1>`                 | 🔴      | 🟢        | 🔴       | Update the entity with the specified primary key of a specific model       |
| `PUT`    | `/api/v1/<model name>/<pk1>/<pk2>/.../<pkN>` | 🔴      | 🔴        | 🟢       | Update the entity with the specified primary key of a specific model       |
| `DELETE` | `/api/v1/<model name>/<pk1>`                 | 🔴      | 🟢        | 🔴       | Delete the entity with the specified primary key of a specific model       |
| `DELETE` | `/api/v1/<model name>/<pk1>/<pk2>/.../<pkN>` | 🔴      | 🔴        | 🟢       | Delete the entity with the specified primary key of a specific model       |

> 📘**Filtering is applicable**
>
> See more about filtering [bellow](#filtering).

### Code samples

```csharp
[GenericEntity(nameof(Id))]
[GenericController]
public class User
{
    public long Id { get; set; }

    // ...
}
```

## GraphQL

Mark with `[GenericGqlQuery]` and with `[GenericGqlMutation]` attributes the models for which AutoBackend has to
generate GraphQL **queries** and **mutations** accordingly.

> ❗**Disclaimer**
>
> Be noticed that `[GenericGqlQuery]` and `[GenericGqlMutation]` only supports models which also marked
> with `[GenericEntity]`

### Queries

{::comment} TODO {:/comment}

> 📘**Filtering is applicable**
>
> See more about filtering [bellow](#filtering).

### Mutations

## Modeling

{::comment} TODO {:/comment}

## Filtering

{::comment} TODO {:/comment}

### Mark the model properties which AutoBackend has to generate filters for

AutoBackend.SDK can automatically create a filter object model with necessary properties
which also will be standard filter objects with standard set of filter conditions.

### Defaults

By default there are always two filter parameters applicable for any GET request
(which returns list of entities) or GraphQL queries to paginate the response:

- `skipCount`, number
- `takeCount`, number

### Generic

Use `[GenericFilter]` to mark the model property as a property that the generated entity can be filtered by.

```csharp
[GenericEntity(nameof(Id))]
[GenericController]
public class Budget
{
    [GenericFilter]
    public Guid Id { get; set; }

    [GenericFilter]
    public string Name { get; set; }

    [GenericFilter]
    public long? OwnerId { get; set; }

    // ...
}
```

As a result, AutoBackend will build a filter model with a set of parameters which you can use in the API endpoints, such
as `/api/v1/<model name>` or `/api/v1/<model name>/count`, and in the GraphQL queries.

- The filter parameter's name for the API will be generated by pattern: `<property's camelCase-name>.<condition name>`.
- For the GraphQL queries filtering models with the condition properties will be generated.

There are following conditions available:
`equal`, `notEqual`, `greaterThan`, `greaterThanOrEqual`, `lessThan`, `lessThanOrEqual`, `in`, `isNull`, `equal`.