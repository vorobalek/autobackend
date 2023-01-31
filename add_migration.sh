#!/bin/sh
set -x

MIGRATION_NAME=$1

cd ./src/Api || exit

(dotnet ef migrations add "ef_$MIGRATION_NAME" -o Migrations/SqlServer -c SqlServerAutoBackendDbContext || true)
(dotnet ef migrations add "ef_$MIGRATION_NAME" -o Migrations/Postgres -c PostgresAutoBackendDbContext  || true)

cd ../..
exit