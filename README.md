# AutoBackend

| DEVELOPER BUILDS                                                                                                                                                                     | RELEASE BUILDS                                                                                                                                                                                                                                                                                                          |
|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [![MyGet](https://img.shields.io/myget/autobackend-dev/v/AutoBackend.SDK?label=myget&style=for-the-badge)](https://www.myget.org/feed/autobackend-dev/package/nuget/AutoBackend.SDK) | [![MyGet](https://img.shields.io/myget/autobackend/v/AutoBackend.SDK?label=myget&style=for-the-badge)](https://www.myget.org/feed/autobackend/package/nuget/AutoBackend.SDK) [![NuGet](https://img.shields.io/nuget/v/AutoBackend.SDK?label=nuget&style=for-the-badge)](https://www.nuget.org/packages/AutoBackend.SDK) |

This package provides infrastructure templates for facilitating the creation of backend services.
It is a personal project without a commercial foundation and offers no guarantees regarding future development.

# Contribution

If you utilize this package, please consider contributing to this repository.
Adhere to the [Contributor Covenant Code of Conduct](docs/github/CODE_OF_CONDUCT.md) for guidance.

# Features

- [Database](#database)
    - [Schema Modeling](#schema-modeling)
        - [Keyless Entities](#keyless-entities)
        - [Single-PK Entities](#single-pk-entities)
        - [Multi-PK Entities](#multi-pk-entities)
    - [Providers](#providers)
    - [Migrations](#migrations)
        - [Migrate on Startup](#migrate-on-startup)
- [API](#api)
    - [HTTP API](#http-api)
    - [GraphQL](#graphql)
        - [Queries](#queries)
        - [Mutations](#mutations)
    - [Modeling](#modeling)
    - [Filtering](#filtering)
    - _Authorization: currently unsupported (may be introduced in future releases)_

# Initialization

A sample usage scenario is available [here](src/samples/Sample).
Copies of those samples are also provided.

## Initialize AutoBackend from your Program.cs file

```csharp
using AutoBackend.Sdk;

await new AutoBackendHost<Program>().RunAsync(args);
```

## Define models to represent your domain relationships

Refer to the [sample project models](src/samples/Sample/Data) for examples.

```csharp
public class Budget
{
    public Guid Id { get; set; }
    
    //..
}

public class Transaction
{
    public Guid Id { get; set; }
    
    //..
}

public class TransactionVersion
{
    //..
}

public class User
{
    public long Id { get; set; }
    
    //..
}

public class Participating
{
    public long UserId { get; set; }
    
    public Guid BudgetId { get; set; }
    
    //..
}
```

## Configuration of all features is detailed below

---

# Database

Currently supported relational databases (including in-memory) are:

- SqlServer
- Postgres

AutoBackend.SDK will generate tables and establish relationships for your configured entities.

## Schema Modeling

There are typically three types of database tables:

- Keyless,
- Single-column primary key,
- Multi-column primary key.

Depending on the number of columns in your entity, follow the instructions below to enable AutoBackend.SDK to track
changes.

### Keyless Entities

Use the `[GenericEntity]` attribute to designate a model as a keyless entity.

```csharp
[GenericEntity]
public class TransactionVersion
{
    //...
}
```

### Single-PK Entities

Use the `[GenericEntity(<primary key property name>)]` attribute to designate a model as an entity with a single
property primary key.

```csharp
[GenericEntity(
    nameof(Id)
)]
public class Budget
{
    public Guid Id { get; set; }

    // ...
}
```

### Multi-PK Entities

Use the `[GenericEntity(<primary key property names>)]` attribute to designate a model as an entity with a composite
primary key.

> Currently, the maximum number of properties in a composite key is **8**.
>
> It is anticipated that this limit will suffice for most use cases.

```csharp
[GenericEntity(
    nameof(UserId),
    nameof(BudgetId)
)]
public class Participating
{
    public long UserId { get; set; }

    public Guid BudgetId { get; set; }
    
    // ...
}
```

## Providers

For database connection management, configure the "Database" section.

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

The `PrimaryProvider` property accepts one of the following string values:

- `InMemory`,
- `SqlServer`,
- `Postgres`.

The `Providers` property must contain an object with optional properties: `InMemory`, `SqlServer`, or `Postgres`. The
property corresponding to the chosen `PrimaryProvider` value is mandatory.

## Migrations

First, [install Entity Framework Core Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet).
You can create a new migration with one of the following commands executed from the project root directory.

`dotnet ef migrations add "<your migration name>" -o Migrations/SqlServer -c SqlServerGenericDbContext` for SqlServer.

`dotnet ef migrations add "<your migration name>" -o Migrations/Postgres -c PostgresGenericDbContext` for Postgres.

Alternatively, use scripts [to add a new migration](scripts/add_migration.sh)
or [to remove the last migration](scripts/remove_migration.sh) for both database providers.

If you opt not to have AutoBackend manage database migrations (see above), you can perform migrations manually by
executing `dotnet ef database update` from the project root directory.

### Migrate on Startup

If using a relational database (e.g., SqlServer or Postgres), you can configure AutoBackend to either automatically
migrate the database at application startup or not, by passing the `migrateRelationalOnStartup` parameter to
the `RunAsync` method where AutoBackend is initialized. For example:

```csharp
await new AutoBackend.Sdk.AutoBackendHost<Program>().RunAsync(args, migrateRelationalOnStartup: true);
```

# API

AutoBackend.SDK currently supports two types of application interfaces:

- Traditional HTTP API
- Basic GraphQL

## HTTP API

Use the `[GenericController]` attribute on models to generate HTTP API endpoints by AutoBackend.

> ❗**Disclaimer**
>
> The `[GenericController]` attribute is compatible only with models also marked with `[GenericEntity]`.

### Response Container and Endpoints

The current and only API version is v1. API v1 supports JSON and uses the following response container format:

```
{
  "ok": boolean,
  "error_code": number,
  "description": string,
  "request_time_ms": number,
  "result": <response object>
}
```

For detailed information on generated endpoints, access `/swagger`.

### Endpoints

The `[GenericController]` attribute generates the following HTTP API endpoints:

| Method   | Url                                          | Keyless | Single-PK | Multi-PK | Description                                                             |
|----------|----------------------------------------------|---------|-----------|----------|-------------------------------------------------------------------------|
| `GET`    | `/api/v1/<model name>`                       | ✔️      | ✔️        | ✔️       | Retrieve **all** entities of a specific model                           |
| `GET`    | `/api/v1/<model name>/count`                 | ✔️      | ✔️        | ✔️       | Retrieve the **count** of entities of a specific model                  |
| `GET`    | `/api/v1/<model name>/<pk1>`                 | ❌       | ✔️        | ❌        | Retrieve an entity with the requested primary key of a specific model   |
| `GET`    | `/api/v1/<model name>/<pk1>/<pk2>/.../<pkN>` | ❌       | ❌         | ✔️       | Retrieve an entity with the requested composite primary key of a model  |
| `POST`   | `/api/v1/<model name>`                       | ✔️      | ❌         | ❌        | Create a new keyless entity of a specific model                         |
| `POST`   | `/api/v1/<model name>/<pk1>`                 | ❌       | ✔️        | ❌        | Create a new entity with the specified primary key of a specific model  |
| `POST`   | `/api/v1/<model name>/<pk1>/<pk2>/.../<pkN>` | ❌       | ❌         | ✔️       | Create a new entity with the specified composite primary key of a model |
| `PUT`    | `/api/v1/<model name>/<pk1>`                 | ❌       | ✔️        | ❌        | Update an entity with the specified primary key of a specific model     |
| `PUT`    | `/api/v1/<model name>/<pk1>/<pk2>/.../<pkN>` | ❌       | ❌         | ✔️       | Update an entity with the specified composite primary key of a model    |
| `DELETE` | `/api/v1/<model name>/<pk1>`                 | ❌       | ✔️        | ❌        | Delete an entity with the specified primary key of a specific model     |
| `DELETE` | `/api/v1/<model name>/<pk1>/<pk2>/.../<pkN>` | ❌       | ❌         | ✔️       | Delete an entity with the specified composite primary key of a model    |

> 📘**Filtering Capabilities**
>
> Refer to the [following section](#filtering) for more information on filtering.

#### Code Samples

```csharp
[GenericEntity(
    nameof(Id)
)]
[GenericController]
public class User
{
    public long Id { get; set; }

    // ...
}
```

## GraphQL

Use the `[GenericGqlQuery]` and `[GenericGqlMutation]` attributes on models to generate GraphQL **queries** and *
*mutations**, respectively.

> ❗**Disclaimer**
>
> The `[GenericGqlQuery]` and `[GenericGqlMutation]` attributes are compatible only with models also marked
> with `[GenericEntity]`.

For detailed information on generated queries and mutations, access `/graphql`.

### Queries

The `[GenericGqlQuery]` attribute generates the following GraphQL queries:

| Query   | Arguments                                  | Keyless | Single-PK | Multi-PK | Description                                                  |
|---------|--------------------------------------------|---------|-----------|----------|--------------------------------------------------------------|
| `all`   | `filter`: generic filter input model       | ✔️      | ✔️        | ✔️       | Retrieve **all** entities of a specific model                |
| `count` | `filter`: generic filter input model       | ✔️      | ✔️        | ✔️       | Retrieve the **count** of entities of a specific model       |
| `byKey` | `key`: entity primary key                  | ❌       | ✔️        | ❌        | Retrieve an entity with the requested primary key of a model |
| `byKey` | `key1`, `key2`, ..., `keyN`: entity PK-set | ❌       | ❌         | ✔️       | Retrieve an entity with the requested composite primary key  |

> 📘**Filtering Capabilities**
>
> Refer to the [following section](#filtering) for more information on filtering.

#### Code Samples

```csharp
[GenericEntity(
    nameof(Id)
)]
[GenericGqlQuery]
public class User
{
    public long Id { get; set; }

    // ...
}
```

### Mutations

The `[GenericGqlMutation]` attribute generates the following GraphQL mutations:

| Mutation | Arguments                                                                         | Keyless | Single-PK | Multi-PK | Description                                                   |
|----------|-----------------------------------------------------------------------------------|---------|-----------|----------|---------------------------------------------------------------|
| `create` | `request`: generic entity input model                                             | ✔️      | ❌         | ❌        | Create a new keyless entity of a specific model               |
| `create` | `key`: entity primary key, `request`: generic entity input model                  | ❌       | ✔️        | ❌        | Create a new entity with the specified primary key of a model |
| `create` | `key1`, `key2`, ..., `keyN`: entity PK-set, `request`: generic entity input model | ❌       | ❌         | ✔️       | Create a new entity with the specified composite primary key  |
| `update` | `key`: entity primary key, `request`: generic entity input model                  | ❌       | ✔️        | ❌        | Update an entity with the specified primary key of a model    |
| `update` | `key1`, `key2`, ..., `keyN`: entity PK-set, `request`: generic entity input model | ❌       | ❌         | ✔️       | Update an entity with the specified composite primary key     |
| `delete` | `key`: entity primary key (for PK-holder entities only)                           | ❌       | ✔️        | ❌        | Delete an entity with the specified primary key               |
| `delete` | `key1`, `key2`, ..., `keyN`: entity PK-set                                        | ❌       | ❌         | ✔️       | Delete an entity with the specified composite primary key     |

#### Code Samples

```csharp
[GenericEntity(
    nameof(Id)
)]
[GenericGqlMutation]
public class User
{
    public long Id { get; set; }

    // ...
}
```

## Modeling

AutoBackend.SDK generates request and response models for any entity with configured HTTP API or GraphQL generation.
These models include all original entity properties, unless specific properties are explicitly included in request or
response models, in which case non-specified properties will be omitted.

### Request Models

The `[GenericRequest]` attribute defines which properties are permitted for reflection from the request model to the
entity.

#### Code Samples

```csharp
[GenericEntity(
    nameof(Id)
)]
[GenericController]
[GenericGqlQuery]
[GenericGqlMutation]
[GenericRequest(
    nameof(Id),
    nameof(UserId),
    nameof(BudgetId),
    nameof(Amount),
    nameof(DateTimeUtc),
    nameof(Comment),
    nameof(SecretKey)
)]
public class Transaction
{
    // ...
}
```

### Response Models

The `[GenericResponse]` attribute defines which properties are permitted for reflection from the response model to the
entity.

#### Code Samples

```csharp
[GenericEntity(
    nameof(Id)
)]
[GenericController]
[GenericGqlQuery]
[GenericGqlMutation]
[GenericResponse(
    nameof(Id),
    nameof(UserId),
    nameof(BudgetId),
    nameof(Amount),
    nameof(DateTimeUtc),
    nameof(Comment)
)]
public class Transaction
{
    // ...
}
```

## Filtering

AutoBackend.SDK generates filter models for any entity with configured HTTP API or GraphQL generation. By default, these
models include only pagination management properties. To include specific entity properties in the filter model, use
the `[GenericFilter]` attribute.

### Defaults

By default, two filter parameters are always available for any GET request (returning a list of entities) or GraphQL
queries, to manage pagination:

- `skipCount`: number
- `takeCount`: number

### Generic

The `[GenericFilter]` attribute designates a model property as filterable in the generated entity.

```csharp
[GenericEntity(
    nameof(Id)
)]
[GenericController]
[GenericGqlQuery]
[GenericGqlMutation]
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

Consequently, AutoBackend will construct a filter model with parameters that can be utilized in API endpoints
like `/api/v1/<model name>` or `/api/v1/<model name>/count`, and in GraphQL queries.

- API filter parameter names are generated following the pattern: `<property's camelCase-name>.<condition name>`.
- GraphQL queries will have filtering models with condition properties generated.

The following conditions are supported:
`equal`, `notEqual`, `greaterThan`, `greaterThanOrEqual`, `lessThan`, `lessThanOrEqual`, `in`, `isNull`, `equal`.