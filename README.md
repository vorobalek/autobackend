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
- [Best practices to use](#be-noticed-of-best-practices)
    - [GenericDbContext](#genericdbcontext-is-way-better-than-any-other)

# Examples

The basic using scenario can be found in [the Api project](https://github.com/vorobalek/autobackend/tree/main/src/Api).
Also, here is a copy of that samples.

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

    public string Title { get; set; } = null!;

    public string? Artist { get; set; }

    public int Score { get; set; } = 0;
}

public class Song
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Author { get; set; }

    public string? Text { get; set; }

    public decimal Price { get; set; }
}

public class AlbumContent
{
    [ForeignKey(nameof(Album))]
    public Guid AlbumId { get; set; }

    [ForeignKey(nameof(Song))]
    public Guid SongId { get; set; }

    [DeleteBehavior(DeleteBehavior.Cascade)]
    public Album Album { get; set; } = null!;

    [DeleteBehavior(DeleteBehavior.Cascade)]
    public Song Song { get; set; } = null!;
}

public class AlbumSet
{
    [ForeignKey(nameof(Album1))]
    public Guid Album1Id { get; set; }

    [ForeignKey(nameof(Album2))]
    public Guid? Album2Id { get; set; }

    [ForeignKey(nameof(Album3))]
    public Guid? Album3Id { get; set; }

    [ForeignKey(nameof(Album4))]
    public Guid Album4Id { get; set; }

    [ForeignKey(nameof(Album5))]
    public Guid? Album5Id { get; set; }

    [ForeignKey(nameof(Album6))]
    public Guid? Album6Id { get; set; }

    [ForeignKey(nameof(Album7))]
    public Guid? Album7Id { get; set; }

    [ForeignKey(nameof(Album8))]
    public Guid? Album8Id { get; set; }

    [DeleteBehavior(DeleteBehavior.Cascade)]
    public Album Album1 { get; set; } = null!;

    [DeleteBehavior(DeleteBehavior.SetNull)]
    public Album? Album2 { get; set; }

    [DeleteBehavior(DeleteBehavior.SetNull)]
    public Song? Album3 { get; set; }

    [DeleteBehavior(DeleteBehavior.SetNull)]
    public Song? Album4 { get; set; }

    [DeleteBehavior(DeleteBehavior.SetNull)]
    public Song? Album5 { get; set; }

    [DeleteBehavior(DeleteBehavior.SetNull)]
    public Song? Album6 { get; set; }

    [DeleteBehavior(DeleteBehavior.SetNull)]
    public Song? Album7 { get; set; }

    [DeleteBehavior(DeleteBehavior.SetNull)]
    public Song? Album8 { get; set; }

    public string Name { get; set; } = null!;
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
public class Song
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
    nameof(AlbumId),
    nameof(SongId)
)]
public class AlbumContent
{
    [ForeignKey(nameof(Album))]
    public Guid AlbumId { get; set; }

    [ForeignKey(nameof(Song))]
    public Guid SongId { get; set; }
    
    // ...
}

[GenericEntity(
    nameof(Album1Id),
    nameof(Album2Id),
    nameof(Album3Id),
    nameof(Album4Id),
    nameof(Album5Id),
    nameof(Album6Id),
    nameof(Album7Id),
    nameof(Album8Id)
)]
public class AlbumSet
{
    [ForeignKey(nameof(Album1))]
    public Guid Album1Id { get; set; }

    [ForeignKey(nameof(Album2))]
    public Guid? Album2Id { get; set; }

    [ForeignKey(nameof(Album3))]
    public Guid? Album3Id { get; set; }

    [ForeignKey(nameof(Album4))]
    public Guid Album4Id { get; set; }

    [ForeignKey(nameof(Album5))]
    public Guid? Album5Id { get; set; }

    [ForeignKey(nameof(Album6))]
    public Guid? Album6Id { get; set; }

    [ForeignKey(nameof(Album7))]
    public Guid? Album7Id { get; set; }

    [ForeignKey(nameof(Album8))]
    public Guid? Album8Id { get; set; }

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
  public class Song
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
    nameof(AlbumId),
    nameof(SongId)
  )]
  [GenericControllerV1]
  public class AlbumContent
  {
      // ...
  }
  
  [GenericEntity(
    nameof(Album1Id),
    nameof(Album2Id),
    nameof(Album3Id),
    nameof(Album4Id),
    nameof(Album5Id),
    nameof(Album6Id),
    nameof(Album7Id),
    nameof(Album8Id)
  )]
  [GenericControllerV1]
  public class AlbumSet
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

    [GenericFilter] public string Title { get; set; } = null!;

    [GenericFilter] public string? Artist { get; set; }

    [GenericFilter] public int Score { get; set; } = 0;
}
```

As a result, AutoBackend will build a filter model that you can use in the API endpoints, such
as `/api/v1/<model name>/filter` or `/api/v1/<model name>/filter/count`. For the example above, the filter model looks
like that:

```
{
  "id": {
    "equal": string | null | undefined,
    "notEqual": string | null | undefined,
    "isNull": boolean | null | undefined,
    "greaterThan": string | null | undefined,
    "greaterThanOrEqual": string | null | undefined,
    "lessThan": string | null | undefined,
    "lessThanOrEqual": string | null | undefined,
    "in": [
      string | null | undefined,
      string | null | undefined
    ]
  },
  "title": {
    "equal": string | null | undefined,
    "notEqual": string | null | undefined,
    "isNull": boolean | null | undefined,
    "greaterThan": string | null | undefined,
    "greaterThanOrEqual": string | null | undefined,
    "lessThan": string | null | undefined,
    "lessThanOrEqual": string | null | undefined,
    "in": [
      string | null | undefined,
      string | null | undefined
    ]
  },
  "artist": {
    "equal": string | null | undefined,
    "notEqual": string | null | undefined,
    "isNull": boolean | null | undefined,
    "greaterThan": string | null | undefined,
    "greaterThanOrEqual": string | null | undefined,
    "lessThan": string | null | undefined,
    "lessThanOrEqual": string | null | undefined,
    "in": [
      string | null | undefined,
      string | null | undefined
    ]
  },
  "score": {
    "equal": number | null | undefined,
    "notEqual": number | null | undefined,
    "isNull": boolean | null | undefined,
    "greaterThan": number | null | undefined,
    "greaterThanOrEqual": number | null | undefined,
    "lessThan": number | null | undefined,
    "lessThanOrEqual": number | null | undefined,
    "in": [
      number | null | undefined,
      number | null | undefined
    ]
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

`dotnet ef migrations add "<your migration name>" -o Migrations/SqlServer -c SqlServerAutoBackendDbContext` - if you use
SqlServer.

`dotnet ef migrations add "<your migration name>" -o Migrations/Postgres -c PostgresAutoBackendDbContext` - if you use
Postgres.

Or you can create scripts
for [adding a new migration](https://github.com/vorobalek/autobackend/blob/main/add_migration.sh)
or [removing the last migration](https://github.com/vorobalek/autobackend/blob/main/remove_migration.sh) for both
database providers.

Finally, suppose you did not choose to delegate the database migrating to AutoBackend (see above). In that case, you can
migrate it yourself, executing `dotnet ef database update` from the root of the project folder.

## Be noticed of best practices

### GenericDbContext is way better than any other

Despite the database provider you have chosen, if you need to access the `DbContext` directly from your code, it will be
the best practice to inject the `GenericDbContext` into your services. This way, you can switch between any database
providers offered by `AutoBackend.SDK` simply by changing one line in your application config.