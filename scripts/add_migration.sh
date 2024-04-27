#!/bin/sh
set -x

MIGRATION_NAME=$1

cd ./../src/samples/Sample || exit

(dotnet ef migrations add "ef_$MIGRATION_NAME" -o Migrations/SqlServer -c SqlServerGenericDbContext || true)
(dotnet ef migrations add "ef_$MIGRATION_NAME" -o Migrations/Postgres -c PostgresGenericDbContext  || true)

cd ../../../scripts || exit
exit