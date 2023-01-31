#!/bin/sh
set -x

cd ./src/Api || exit

(dotnet ef migrations remove -c SqlServerAutoBackendDbContext || true)
(dotnet ef migrations remove -c PostgresAutoBackendDbContext || true)

cd ../..
exit