#!/bin/sh

BASEDIR=$(dirname "$0")
set -x
MIGRATION_NAME=$1
cd "$BASEDIR/../src/samples/Sample" || exit
(dotnet ef migrations add "ef_$MIGRATION_NAME" -o Migrations/SqlServer -c SqlServerGenericDbContext || true)
(dotnet ef migrations add "ef_$MIGRATION_NAME" -o Migrations/Postgres -c PostgresGenericDbContext  || true)
exit