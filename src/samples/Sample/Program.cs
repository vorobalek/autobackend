using AutoBackend.Sdk;

await new AutoBackendHost<Program>().RunAsync(args, migrateRelationalOnStartup: true);