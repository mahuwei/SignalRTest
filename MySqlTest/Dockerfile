FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY MySqlTest.csproj MySqlTest/
RUN dotnet restore MySqlTest/MySqlTest.csproj
WORKDIR /src/MySqlTest
COPY . .

RUN dotnet build MySqlTest.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish MySqlTest.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MySqlTest.dll"]