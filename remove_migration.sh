#!/bin/sh
set -x

cd ./src/Api || exit

(dotnet ef migrations remove -c SqlServerGenericDbContext || true)
(dotnet ef migrations remove -c PostgresGenericDbContext || true)

cd ../..
exit