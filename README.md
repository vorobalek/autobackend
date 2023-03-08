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

- [Database schema modeling](#mark-models-which-autobackend-has-to-generate-tables-in-the-database-for)
- [CRUD API modeling](#mark-models-which-autobackend-has-to-generate-api-endpoints-for)
- [Filtering models for API](#mark-model-properties-which-autobackend-has-to-generate-filters-for)
- [Cross-database provider support](#make-sure-you-have-configured-your-application-database-connection-correctly)
- [Always up-to-date database schema](#make-sure-you-have-configured-your-application-startup-correctly)

# Examples

The basic using scenario can be found in [the Api project](https://github.com/vorobalek/autobackend/tree/main/src/Api). Also, here is a copy of that samples.

## Initialize AutoBackend from your Program.cs file

```csharp
await new AutoBackend.Sdk.AutoBackendHost<Program>().RunAsync(args);
```

## Create models you need to describe your domain relations

```csharp
public class Note
{
    public string Content { get; set; } = null!;
}

public class Album
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Artist { get; set; }
}

public class Book
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Author { get; set; }
    
    public decimal Price { get; set; }
}

public class Book2Albums
{
    [ForeignKey(nameof(Book))]
    public Guid BookId { get; set; }
    
    public Book Book { get; set; }

    [ForeignKey(nameof(Album))]
    public Guid AlbumId { get; set; }
    
    public Album Album { get; set; }
}

public class BookShelve
{
    public string? Name { get; set; }

    [ForeignKey(nameof(Book1))]
    public Guid Book1Id { get; set; }

    public Book Book1 { get; set; }

    [ForeignKey(nameof(Book2))]
    public Guid Book2Id { get; set; }

    public Book Book2 { get; set; }

    [ForeignKey(nameof(Book3))]
    public Guid Book3Id { get; set; }

    public Book Book3 { get; set; }

    [ForeignKey(nameof(Book4))]
    public Guid Book4Id { get; set; }

    public Book Book4 { get; set; }

    [ForeignKey(nameof(Book5))]
    public Guid Book5Id { get; set; }

    public Book Book5 { get; set; }

    [ForeignKey(nameof(Book6))]
    public Guid Book6Id { get; set; }

    public Book Book6 { get; set; }

    [ForeignKey(nameof(Book7))]
    public Guid Book7Id { get; set; }

    public Book Book7 { get; set; }

    [ForeignKey(nameof(Book8))]
    public Guid Book8Id { get; set; }

    public Book Book8 { get; set; }
}
```

## Mark models which AutoBackend has to generate tables in the database for

- Use `[GenericEntity]` to mark the model as a keyless entity.

```csharp
[GenericEntity]
public class Note
{
    // ...
}
```

- Use `[GenericEntity(<primary key property name>)]` to mark the model as an entity with the primary key displayed as a
  single property.

```csharp
[GenericEntity(
    nameof(Id)
)]
public class Album
{
    public Guid Id { get; set; }

    // ...
}

[GenericEntity(
    nameof(Id)
)]
public class Book
{
    public Guid Id { get; set; }

    // ...
}
```

-

Use `[GenericEntity(<first complex primary key property name>, <second complex primary key property name>, ...,  <N-th complex primary key>)]`
to mark the model as an entity with the primary key displayed as a complex set of properties.

```csharp
[GenericEntity(
    nameof(BookId),
    nameof(AlbumId)
)]
public class Book2Albums
{
    [ForeignKey(nameof(Book))]
    public Guid BookId { get; set; }

    [ForeignKey(nameof(Album))]
    public Guid AlbumId { get; set; }
    
    // ...
}

[GenericEntity(
    nameof(Book1Id),
    nameof(Book2Id),
    nameof(Book3Id),
    nameof(Book4Id),
    nameof(Book5Id),
    nameof(Book6Id),
    nameof(Book7Id),
    nameof(Book8Id)
)]
public class BookShelve
{
    [ForeignKey(nameof(Book1))]
    public Guid Book1Id { get; set; }

    [ForeignKey(nameof(Book2))]
    public Guid Book2Id { get; set; }

    [ForeignKey(nameof(Book3))]
    public Guid Book3Id { get; set; }

    [ForeignKey(nameof(Book4))]
    public Guid Book4Id { get; set; }

    [ForeignKey(nameof(Book5))]
    public Guid Book5Id { get; set; }

    [ForeignKey(nameof(Book6))]
    public Guid Book6Id { get; set; }

    [ForeignKey(nameof(Book7))]
    public Guid Book7Id { get; set; }

    [ForeignKey(nameof(Book8))]
    public Guid Book8Id { get; set; }

    // ...
}
```

## Mark models which AutoBackend has to generate API endpoints for

The latest API version is v1. APIv1 supports JSON only, and its output uses contract:

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

### ❗️❗️❗️ Be noticed that [GenericControllerV1] supports only models also marked with [GenericEntity]

Use `[GenericControllerV1]` to generate for:

- Keyless entity:\
  **GET** `/api/v1/<model name>` - returns all entities\
  **GET** `/api/v1/<model name>/count` - returns count of all entities\
  **POST** `/api/v1/<model name>/filter` - return filtered entities\
  **POST** `/api/v1/<model name>/filter/count` - return filtered entities count

  ```csharp
  [GenericEntity]
  [GenericControllerV1]
  public class Note
  {
      // ...
  }
  ```

- Entity with a single primary key:
  Same as for keyless entity:\
  **GET** `/api/v1/<model name>` - returns all entities\
  **GET** `/api/v1/<model name>/count` - returns count of all entities\
  **POST** `/api/v1/<model name>/filter` - return filtered entities\
  **POST** `/api/v1/<model name>/filter/count` - return filtered entities count

  And extra\
  **GET** `/api/v1/<model name>/{key}` - returns a specific entity by the primary key\
  **POST** `/api/v1/<model name>/{key}` - creates a specific entity by the primary key\
  **PUT** `/api/v1/<model name>/{key}` - updates a specific entity by the primary key\
  **DELETE** `/api/v1/<model name>/{key}` - deletes a specific entity by the primary key

  ```csharp
  [GenericEntity(
      nameof(Id)
  )]
  [GenericControllerV1]
  public class Album
  {
      // ...
  }

  [GenericEntity(
      nameof(Id)
  )]
  [GenericControllerV1]
  public class Book
  {
      // ...
  }
  ```

- Entity with a complex primary key:
  Same as for keyless entity:\
  **GET** `/api/v1/<model name>` - returns all entities\
  **GET** `/api/v1/<model name>/count` - returns count of all entities\
  **POST** `/api/v1/<model name>/filter` - return filtered entities\
  **POST** `/api/v1/<model name>/filter/count` - return filtered entities count

  And extra\
  **GET** `/api/v1/<model name>/{key1}/{key2}/.../{keyN}` - returns a specific entity by the complex primary key\
  **POST** `/api/v1/<model name>/{key1}/{key2}/.../{keyN}` - creates a specific entity by the complex primary key\
  **PUT** `/api/v1/<model name>/{key1}/{key2}/.../{keyN}` - updates a specific entity by the complex primary key\
  **DELETE** `/api/v1/<model name>/{key1}/{key2}/.../{keyN}` - deletes a specific entity by the complex primary key

  ```csharp
  [GenericEntity(
      nameof(BookId),
      nameof(AlbumId)
  )]
  [GenericControllerV1]
  public class Book2Albums
  {
      // ...
  }
  
  [GenericEntity(
      nameof(Book1Id),
      nameof(Book2Id),
      nameof(Book3Id),
      nameof(Book4Id),
      nameof(Book5Id),
      nameof(Book6Id),
      nameof(Book7Id),
      nameof(Book8Id)
  )]
  [GenericControllerV1]
  public class BookShelve
  {
      // ...
  }
  ```

## Mark model properties which AutoBackend has to generate filters for

- Use `[GenericFilter]` to mark the model property as a property that the generated entity can be filtered by.

```csharp
[GenericEntity(
    nameof(Id)
)]
[GenericControllerV1]
public class Album
{
    [GenericFilter] public Guid Id { get; set; }

    public string? Title { get; set; }

    [GenericFilter] public string? Artist { get; set; }
}
```

As a result, AutoBackend will build a filter model that you can use in the API endpoints, such
as `/api/v1/<model name>/filter` or `/api/v1/<model name>/filter/count`. For the example above, the filter model looks
that:

```
{
  "id": {
    "equal": "string" | undefined | null,
    "notEqual": "string" | undefined | null,
    "isNull": true | undefined | null,
    "greaterThan": "string" | undefined | null,
    "greaterThanOrEqual": "string" | undefined | null,
    "lessThan": "string" | undefined | null,
    "lessThanOrEqual": "string" | undefined | null,
    "in": [
      "string"
    ] | undefined | null
  },
  "artist": {
    "equal": "string" | undefined | null,
    "notEqual": "string" | undefined | null,
    "isNull": true | undefined | null,
    "greaterThan": "string" | undefined | null,
    "greaterThanOrEqual": "string" | undefined | null,
    "lessThan": "string" | undefined | null,
    "lessThanOrEqual": "string" | undefined | null,
    "in": [
      "string"
    ] | undefined | null
  },
  "score": {
    "equal": 0 | undefined | null,
    "notEqual": 0 | undefined | null,
    "isNull": true | undefined | null,
    "greaterThan": 0 | undefined | null,
    "greaterThanOrEqual": 0 | undefined | null,
    "lessThan": 0 | undefined | null,
    "lessThanOrEqual": 0 | undefined | null,
    "in": [
      0
    ] | undefined | null
  }
}
```

It contains properties with the same names as the model properties marked with `[GenericFilter]`. Each property is an
object with filter parameters. You can see that each filter parameter type is the same as the model property marked with
the `[GenericFilter]`. All filter parameters are optional.

## Make sure you have configured your application database connection correctly

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

- If you would like to use the InMemory database, you shall fill in the "Database" configuration section like this:
  ```json
  "Database": {
    "PrimaryProvider": "InMemory",
    "Providers": {
      "InMemory": "<InMemory database name>",
    }
  }
  ```
- If you would like to use the InMemory database, you shall fill in the "Database" configuration section like this:
  ```json
  "Database": {
    "PrimaryProvider": "SqlServer",
    "Providers": {
      "SqlServer": "<SqlServer database connection string>",
    }
  }
  ```
  ```
- If you would like to use the InMemory database, you shall fill in the "Database" configuration section like this:
  ```json
  "Database": {
    "PrimaryProvider": "Postgres",
    "Providers": {
      "Postgres": "<Postgres database connection string in the Npgsql format>",
    }
  }
  ```

## Make sure you have configured your application startup correctly

Suppose you use a relational database (like SqlServer or Postgres). In that case, you can let AutoBackend know whether
it has to migrate the database automatically on the application startup or doesn't, passing
the `migrateRelationalOnStartup` parameter to the `RunAsync` method in the place you call it to initialize AutoBackend.
Here is an example:

```csharp
await new AutoBackend.Sdk.AutoBackendHost<Program>().RunAsync(args, migrateRelationalOnStartup: true);
```

## Migrate the database schema

First, you must [install Entity Framework Core Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet). After that,
you can create a new migration using one of the following commands executed from the root of the project folder.

`dotnet ef migrations add "<your migration name>" -o Migrations/SqlServer -c SqlServerAutoBackendDbContext` - in case
you use the SqlServer.

`dotnet ef migrations add "<your migration name>" -o Migrations/Postgres -c PostgresAutoBackendDbContext` - in case you
use Postgres.

Finally, suppose you did not choose to delegate the database migrating to AutoBackend (see above). In that case, you can
migrate it yourself, executing `dotnet ef database update` from the root of the project folder.
