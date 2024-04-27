FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS restore
WORKDIR /sln
COPY ["src/Api/Api.csproj", "src/Api/"]
COPY ["lib/AutoBackend.Sdk/AutoBackend.Sdk.csproj", "lib/AutoBackend.Sdk/"]
RUN dotnet restore "src/Api/Api.csproj"

FROM restore AS build
COPY . .
WORKDIR "/sln/src/Api/"
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
