# AutoBackend

| DEVELOPER BUILDS                                                                                                                                                                     | RELEASE BUILDS                                                                                                                                                                                                                                                                                                          |
|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [![MyGet](https://img.shields.io/myget/autobackend-dev/v/AutoBackend.SDK?label=myget&style=for-the-badge)](https://www.myget.org/feed/autobackend-dev/package/nuget/AutoBackend.SDK) | [![MyGet](https://img.shields.io/myget/autobackend/v/AutoBackend.SDK?label=myget&style=for-the-badge)](https://www.myget.org/feed/autobackend/package/nuget/AutoBackend.SDK) [![NuGet](https://img.shields.io/nuget/v/AutoBackend.SDK?label=nuget&style=for-the-badge)](https://www.nuget.org/packages/AutoBackend.SDK) |

This package provides infrastructure templates to facilitate the creation of backend services.
This is a personal project with no commercial backing and offers no guarantees regarding future development.

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
    - [Authorization](#authorization)

# Initialization

A sample usage scenario is available [here](src/samples/Sample).

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

## Feature Configuration

All features are detailed in the sections below.

---

# Database

The following relational databases are currently supported (including in-memory):

- SqlServer
- Postgres

AutoBackend.SDK will generate tables and establish relationships for your configured entities.

## Schema Modeling

There are typically three types of database tables:

- Keyless,
- Single-column primary key,
- Multi-column primary key.

Depending on the number of primary key columns in your entity, follow the instructions below to enable AutoBackend.SDK to track changes.

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

Use the `[GenericEntity(<primary key property name>)]` attribute to designate a model as an entity with a single-column primary key.

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
> This limit should be sufficient for most use cases.

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

To configure database connection management, set up the "Database" section in your configuration.

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

The `Providers` property must contain an object with optional properties: `InMemory`, `SqlServer`, or `Postgres`. The property corresponding to the selected `PrimaryProvider` value is required.

## Migrations

First, [install Entity Framework Core Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet).
You can create a new migration by executing one of the following commands from the project root directory:

- For SqlServer: `dotnet ef migrations add "<your migration name>" -o Migrations/SqlServer -c SqlServerGenericDbContext`
- For Postgres: `dotnet ef migrations add "<your migration name>" -o Migrations/Postgres -c PostgresGenericDbContext`

Alternatively, you can use the provided scripts [to add a new migration](scripts/add_migration.sh) or [to remove the last migration](scripts/remove_migration.sh) for both database providers.

If you choose not to use AutoBackend's migration management (see above), you can perform migrations manually by executing `dotnet ef database update` from the project root directory.

### Migrate on Startup

When using a relational database (e.g., SqlServer or Postgres), you can configure AutoBackend to automatically migrate the database at application startup by passing the `migrateRelationalOnStartup` parameter to the `RunAsync` method during AutoBackend initialization. For example:

```csharp
await new AutoBackend.Sdk.AutoBackendHost<Program>().RunAsync(args, migrateRelationalOnStartup: true);
```

# API

AutoBackend.SDK currently supports two types of API interfaces:

- HTTP REST API
- GraphQL

## HTTP API

Apply the `[GenericController]` attribute to your models to have AutoBackend generate HTTP API endpoints.

> ❗**Disclaimer**
>
> The `[GenericController]` attribute is compatible only with models also marked with `[GenericEntity]`.

### Response Container and Endpoints

The current API version is v1. API v1 uses JSON and follows the following response container format:

```
{
  "ok": boolean,
  "error_code": number,
  "description": string,
  "request_time_ms": number,
  "result": <response object>
}
```

For detailed information on generated endpoints, visit `/swagger`.

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

Apply the `[GenericGqlQuery]` and `[GenericGqlMutation]` attributes to your models to generate GraphQL **queries** and **mutations**, respectively.

> ❗**Disclaimer**
>
> The `[GenericGqlQuery]` and `[GenericGqlMutation]` attributes are compatible only with models also marked
> with `[GenericEntity]`.

For detailed information on generated queries and mutations, visit `/graphql`.

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

AutoBackend.SDK automatically generates request and response models for any entity with HTTP API or GraphQL generation configured.
By default, these models include all original entity properties. However, if you explicitly specify properties using the `[GenericRequest]` or `[GenericResponse]` attributes, only those specified properties will be included, and all others will be omitted.

### Request Models

The `[GenericRequest]` attribute specifies which properties can be mapped from the request model to the entity.

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

The `[GenericResponse]` attribute specifies which properties can be mapped from the entity to the response model.

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

## Authorization

AutoBackend.SDK supports permission-based authorization using JWT tokens. You can control access to CRUD operations at both entity and property levels.

### Configuration

Configure JWT settings in your `appsettings.json`:

```json
"Jwt": {
  "PublicKey": "-----BEGIN PUBLIC KEY-----\n...\n-----END PUBLIC KEY-----",
  "ValidIssuer": "AutoBackendServer",
  "ValidAudience": "AutoBackendUser"
}
```

- `PublicKey`: RSA public key in PEM format (required) - used to verify JWT token signatures
- `ValidIssuer`: Optional issuer validation
- `ValidAudience`: Optional audience validation

### Permission Attributes

Use permission attributes on entity classes to require authorization for specific operations:

- `[GenericCreatePermission]` - Requires permission to create entities
- `[GenericReadPermission]` - Requires permission to read entities
- `[GenericUpdatePermission]` - Requires permission to update entities
- `[GenericDeletePermission]` - Requires permission to delete entities

#### Code Samples

```csharp
[GenericEntity(
    nameof(Id)
)]
[GenericController]
[GenericGqlQuery]
[GenericGqlMutation]
[GenericCreatePermission]
[GenericReadPermission]
[GenericUpdatePermission]
[GenericDeletePermission]
public class Budget
{
    public Guid Id { get; set; }
    
    // ...
}
```

### Property-Level Permissions

> ⚠️ **Note**: Property-level permissions are currently only processed for **Update** operations.

You can apply permission attributes to individual properties for fine-grained access control during update operations:

```csharp
[GenericEntity(
    nameof(Id)
)]
[GenericController]
[GenericGqlMutation]
[GenericUpdatePermission]
public class Transaction
{
    public Guid Id { get; set; }
    
    [GenericUpdatePermission]
    public string? SecretKey { get; set; }
    
    // ...
}
```

When updating an entity, AutoBackend will check property-level permissions only for properties that are actually being modified. This allows you to restrict access to sensitive fields while allowing updates to other properties.

### JWT Token Format

JWT tokens must be sent in the `Authorization` header with the `Bearer` prefix:

```
Authorization: Bearer <your-jwt-token>
```

The JWT token must contain claims with:
- **Type**: `permissions`
- **Value**: Permission strings in the format `{EntityName}{PermissionType}` or `{EntityName}{PropertyName}{PermissionType}`

#### Permission Name Format

- **Entity permissions**: `{EntityName}{PermissionType}`
  - Examples: `BudgetCreate`, `BudgetRead`, `BudgetUpdate`, `BudgetDelete`
- **Property permissions**: `{EntityName}{PropertyName}{PermissionType}` (only for Update operations)
  - Examples: `TransactionSecretKeyUpdate`

#### Example JWT Claims

```json
{
  "permissions": [
    "BudgetCreate",
    "BudgetRead",
    "BudgetUpdate",
    "BudgetDelete",
    "TransactionSecretKeyUpdate"
  ]
}
```

### Error Handling

When a request lacks required permissions, AutoBackend returns an `Unauthorized` error with details about missing permissions.

## Filtering

AutoBackend.SDK generates filter models for any entity with configured HTTP API or GraphQL generation. By default, these
models include only pagination management properties. To include specific entity properties in the filter model, use
the `[GenericFilter]` attribute.

### Defaults

By default, two filter parameters are always available for any GET request (returning a list of entities) or GraphQL queries to manage pagination:

- `skipCount`: number
- `takeCount`: number

### Custom Filtering

The `[GenericFilter]` attribute marks a model property as filterable in the generated filter model.

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

AutoBackend will construct a filter model with parameters that can be used in API endpoints like `/api/v1/<model name>` or `/api/v1/<model name>/count`, as well as in GraphQL queries.

- API filter parameter names follow the pattern: `<property's camelCase-name>.<condition name>`
- GraphQL queries will have filtering models with condition properties generated

The following filter conditions are supported:
- `equal`
- `notEqual`
- `greaterThan`
- `greaterThanOrEqual`
- `lessThan`
- `lessThanOrEqual`
- `in`
- `isNull`