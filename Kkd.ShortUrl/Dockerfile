FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Kkd.ShortUrl/Kkd.ShortUrl.csproj", "Kkd.ShortUrl/"]
RUN dotnet restore "Kkd.ShortUrl/Kkd.ShortUrl.csproj"
COPY . .
WORKDIR "/src/Kkd.ShortUrl"
RUN dotnet build "Kkd.ShortUrl.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Kkd.ShortUrl.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Kkd.ShortUrl.dll"]