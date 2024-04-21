#!/bin/sh
set -x

cd ./src/Sample || exit

(dotnet ef migrations remove -c SqlServerGenericDbContext || true)
(dotnet ef migrations remove -c PostgresGenericDbContext || true)

cd ../..
exit