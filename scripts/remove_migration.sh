#!/bin/sh
set -x

cd ./../src/samples/Sample || exit

(dotnet ef migrations remove -c SqlServerGenericDbContext || true)
(dotnet ef migrations remove -c PostgresGenericDbContext || true)

cd ../../../scripts || exit
exit