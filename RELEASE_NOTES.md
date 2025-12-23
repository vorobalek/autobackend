### Changelog Summary:

#### Enhancements:

- Released version v0.1.10 with major infrastructure updates.
- Added JWT authorization support with configurable public key validation.
- Refactored AutoBackendHost configuration by consolidating StartupBoilerplate logic inline.
- Enhanced sample project with PostgreSQL as primary database provider.
- Updated README.md with comprehensive authorization documentation.

#### Package Updates:

- Target framework advanced to .NET 10.0 with SDK version 10.0.0.
- Upgraded Microsoft.EntityFrameworkCore packages from 8.0.4 to 10.0.1.
- Updated Npgsql.EntityFrameworkCore.PostgreSQL from version 8.0.2 to 10.0.0.
- Upgraded HotChocolate.AspNetCore from 13.9.0 to 15.1.11.
- Updated HotChocolate.Data.EntityFramework from 13.9.0 to 15.1.11.
- Incremented NSwag.AspNetCore from 14.0.7 to 14.6.3.
- Upgraded Microsoft.NET.Test.Sdk from 17.9.0 to 18.0.1.
- Updated MSTest.TestAdapter from 3.3.1 to 4.0.2.
- Updated MSTest.TestFramework from 3.3.1 to 4.0.2.
- Elevated coverlet.collector version from 6.0.2 to 6.0.4.
- Removed Flurl.Http dependency.

#### Infrastructure Updates:

- Updated Dockerfile base images to .NET 10.0.
- Enhanced GitHub Actions workflows with latest versions.
- Upgraded actions/checkout from v4 to v6.
- Updated actions/setup-dotnet from v4 to v5.
- Incremented tj-actions/changed-files from v41 to v47.
- Upgraded actions/github-script from v7 to v8.
- Enhanced Dependabot configuration with GitHub Actions support.
- Added parallel test execution configuration.

#### Documentation and Organization:

- Expanded README.md with authorization section and improved descriptions.
- Updated sample project configuration with JWT settings.
- Enhanced PostgreSQL connection string with error detail inclusion.