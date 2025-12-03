#!/bin/sh

BASEDIR=$(dirname "$0")
set -x
cd "$BASEDIR/../src/samples/Sample" || exit
(dotnet ef migrations remove -c SqlServerGenericDbContext || true)
(dotnet ef migrations remove -c PostgresGenericDbContext || true)
exit