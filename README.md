# AutoBackend

| DEVELOPER BUILDS                                                                                                                                                                     | RELEASE BUILDS                                                                                                                                                                                                                                                                                                          |
|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [![MyGet](https://img.shields.io/myget/autobackend-dev/v/AutoBackend.SDK?label=myget&style=for-the-badge)](https://www.myget.org/feed/autobackend-dev/package/nuget/AutoBackend.SDK) | [![MyGet](https://img.shields.io/myget/autobackend/v/AutoBackend.SDK?label=myget&style=for-the-badge)](https://www.myget.org/feed/autobackend/package/nuget/AutoBackend.SDK) [![NuGet](https://img.shields.io/nuget/v/AutoBackend.SDK?label=nuget&style=for-the-badge)](https://www.nuget.org/packages/AutoBackend.SDK) |

This package provides the boilerplate infrastructure to create simplified backend services by managing DataBase & API
layers. It is a pet project without a commercial base and any promises about further development. If you would like to
use this package or sources of this package, please let me know by texting me@vorobalek.dev. I would prefer if any of
your scenarios of using this package left consequences in the form of contribution to this repository.

# Roadmap?

Honestly, I do not have enough time to do something about that. I wrote this code while looking for a job just for fun,
but now I have only decided to publish it. Probably, somewhere I will improve it or maybe even add some more features
like GraphQL. I dunno. I have already spent much more time trying to express how it worked than I was supposed to.

# Features

- [Database schema modeling](#mark-the-models-which-autobackend-has-to-generate-tables-in-the-database-for)
    - [Keyless entities](#configure-a-keyless-entity)
    - [Single-PK entities](#configure-a-single-pk-entity)
    - [Multi-PK entities](#configure-a-multi-pk-entity)
- [Cross-database provider support](#choose-the-database-provider-which-is-the-most-suitable-for-you)
    - [In-Memory](#in-memory)
    - [SqlServer](#sqlserver)
    - [PostgreSQL](#postgresql)
- [Always up-to-date database schema](#keep-your-relational-database-schema-always-up-to-date)
- [Full CRUD HTTP API](#mark-the-models-which-autobackend-has-to-generate-http-api-endpoints-for)
- [GraphQL Queries](#mark-the-models-which-autobackend-has-to-generate-graphql-queries-for)
- [GraphQL Mutations](#mark-the-models-which-autobackend-has-to-generate-graphql-mutations-for)
- [Customizable API contracts](#customize-the-data-your-api-consumer-will-see)
- [Filtering models](#mark-the-model-properties-which-autobackend-has-to-generate-filters-for)

# Examples

The basic using scenario can be found in [here](src/Sample).
Also, here are the copies of that samples.

## Initialize AutoBackend from your Program.cs file

```csharp
using AutoBackend.Sdk;

await new AutoBackendHost<Program>().RunAsync(args);
```

## Create the models you need to describe your domain relations

```csharp
public class Budget
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public long? OwnerId { get; set; }
    
    public User? Owner { get; set; }
    
    public ICollection<User> ActiveUsers { get; set; }

    public ICollection<Transaction> Transactions { get; set; }
    
    public ICollection<Participating> Participatings { get; set; }
}

public class Participating
{
    public long UserId { get; set; }
    
    public User User { get; set; }
    
    public Guid BudgetId { get; set; }
    
    public Budget Budget { get; set; }
}

public class Transaction
{
    public Guid Id { get; set; }
    
    public long? UserId { get; set; }
    
    public User? User { get; set; }
    
    public Guid BudgetId { get; set; }
    
    public Budget Budget { get; set; }
    
    public decimal Amount { get; set; }
    
    public DateTime DateTimeUtc { get; set; }

    public string Comment { get; set; }
}

public class TransactionVersion
{
    public Guid TransactionId { get; set; }
    
    public Transaction Transaction { get; set; }
    
    public Guid OriginalTransactionId { get; set; }

    public DateTime VersionDateTimeUtc { get; set; }

    public long? UserId { get; set; }

    public User? User { get; set; }

    public Guid BudgetId { get; set; }

    public Budget Budget { get; set; }

    public decimal Amount { get; set; }

    public DateTime DateTimeUtc { get; set; }

    public string Comment { get; set; }
}

public class User
{
    public long Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public TimeSpan TimeZone { get; set; }
    
    public Guid? ActiveBudgetId { get; set; }
    
    public Budget? ActiveBudget { get; set; }
    
    public ICollection<Budget> OwnedBudgets { get; set; }

    public ICollection<Participating> Participatings { get; set; }
    
    public ICollection<Transaction> Transactions { get; set; }
}
```

## Mark the models which AutoBackend has to generate tables in the database for

### Configure a keyless entity

Use `[GenericEntity]` to mark the model as a keyless entity.

```csharp
[GenericEntity]
public class TransactionVersion
{
    // ...
}
```

### Configure a single-PK entity

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

### Configure a multi-PK entity

Use `[GenericEntity(<first complex primary key property name>, <second complex primary key property name>, ...,  <N-th complex primary key>)]`
to mark the model as an entity with the primary key displayed as a complex set of properties.

```csharp
[GenericEntity(nameof(UserId), nameof(BudgetId))]
public class Participating
{
    public long UserId { get; set; }

    public Guid BudgetId { get; set; }
    
    // ...
}
```

## Choose the database provider which is the most suitable for you

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

The property `PrimaryProvider` allows one of the following string values: `InMemory`,  `SqlServer`, or `Postgres`. The
property `Providers` shall contain an object with optional properties: `InMemory`,  `SqlServer`, or `Postgres`. If you
chose the `PrimaryProvider` value, the property with the same name as that value is required.

For example:

### In-Memory

  ```json
  "Database": {
    "PrimaryProvider": "InMemory",
    "Providers": {
      "InMemory": "<InMemory database name>",
    }
  }
  ```

### SqlServer

  ```json
  "Database": {
    "PrimaryProvider": "SqlServer",
    "Providers": {
      "SqlServer": "<SqlServer database connection string>",
    }
  }
  ```

### PostgreSQL

  ```json
  "Database": {
    "PrimaryProvider": "Postgres",
    "Providers": {
      "Postgres": "<Postgres database connection string in the Npgsql format>",
    }
  }
  ```

## Keep your relational database schema always up-to-date

Suppose you use a relational database (like SqlServer or Postgres). In that case, you can let AutoBackend know whether
it has to migrate the database automatically on the application startup or doesn't, passing
the `migrateRelationalOnStartup` parameter to the `RunAsync` method in the place you call it to initialize AutoBackend.
Here is an example:

```csharp
await new AutoBackend.Sdk.AutoBackendHost<Program>().RunAsync(args, migrateRelationalOnStartup: true);
```

## Mark the models which AutoBackend has to generate HTTP API endpoints for

### Contracts and endpoints

The latest and the only API version now is v1. API v1 supports JSON only, and its output uses contract:

```
{
  "ok": boolean,
  "error_code": number,
  "description": string,
  "request_time_ms": number,
  "result": object
}
```

For more details, you can always request `/swagger` to get the information about all generated endpoints.

> ❗**Disclaimer**
>
> Be noticed that `[GenericController]` only supports models which also marked with `[GenericEntity]`

Use `[GenericController]` to generate the following HTTP APIs:

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
> See more about filtering [bellow](#mark-the-model-properties-which-autobackend-has-to-generate-filters-for).

### Code samples

```csharp
[GenericEntity]
[GenericController]
public class TransactionVersion
{
    // ...
}
```

```csharp
[GenericEntity(nameof(Id))]
[GenericController]
public class Budget
{
    public Guid Id { get; set; }

    // ...
}
```

```csharp
[GenericEntity(nameof(UserId), nameof(BudgetId))]
[GenericController]
public class Participating
{
    public long UserId { get; set; }

    public Guid BudgetId { get; set; }
    
    // ...
}
```

## Mark the models which AutoBackend has to generate GraphQL queries for

TODO `[GenericGqlQuery]`

## Mark the models which AutoBackend has to generate GraphQL mutations for

TODO `[GenericGqlMutation]`

## Customize the data your API consumer will see

TODO `[GenericRequest]` & `[GenericResponse]`

## Mark the model properties which AutoBackend has to generate filters for

AutoBackend.SDK can automatically create a filter object model with necessary properties
which also will be standard filter objects with standard set of filter conditions.

### Defaults

By default there are always two filter parameters applicable for any GET request
(which returns list of entities) to paginate request response:

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
as `/api/v1/<model name>` or `/api/v1/<model name>/count`.

The filter parameter's name will be generated by pattern: `<property's camelCase-name>.<condition name>`.

There are following conditions available:
`equal`, `notEqual`, `greaterThan`, `greaterThanOrEqual`, `lessThan`, `lessThanOrEqual`, `in`, `isNull`, `equal`.

For the example here are the **optional** query parameters will be available to apply filtering in th query:

- For `Budget.Id`
    - `id.equal`, string
    - `id.notEqual`, string
    - `id.greaterThan`, string
    - `id.greaterThanOrEqual`, string
    - `id.lessThan`, string
    - `id.lessThanOrEqual`, string
    - `id.in`, array of strings
    - `id.isNull`, boolean
    - `id.equal`, string
- For `Budget.Name`
    - `name.equal`, string
    - `name.notEqual`, string
    - `name.greaterThan`, string
    - `name.greaterThanOrEqual`, string
    - `name.lessThan`, string
    - `name.lessThanOrEqual`, string
    - `name.in`, array of strings
    - `name.isNull`, boolean
    - `name.equal`, string
- For `Budget.OwnerId`
    - `ownerId.equal`, number
    - `ownerId.notEqual`, number
    - `ownerId.greaterThan`, number
    - `ownerId.greaterThanOrEqual`, number
    - `ownerId.lessThan`, number
    - `ownerId.lessThanOrEqual`, number
    - `ownerId.in`, array of numbers
    - `ownerId.isNull`, number
    - `ownerId.equal`, number

## Migrate the database schema

First, you must [install Entity Framework Core Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet). After that,
you can create a new migration using one of the following commands executed from the root of the project folder.

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