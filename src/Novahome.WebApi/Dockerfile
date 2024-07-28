FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

RUN apt-get update -y && apt-get install curl -y

USER $APP_UID

WORKDIR /app
EXPOSE 8080
EXPOSE 8081
HEALTHCHECK --interval=3s --timeout=2s --start-period=10s --retries=3 CMD curl --fail http://localhost:8080/healthz || exit 1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Novahome.WebApi/Novahome.WebApi.csproj", "src/Novahome.WebApi/"]
COPY ["src/Novahome.Application/Novahome.Application.csproj", "src/Novahome.Application/"]
COPY ["src/Novahome.Domain/Novahome.Domain.csproj", "src/Novahome.Domain/"]
COPY ["src/Novahome.Infrastructure/Novahome.Infrastructure.csproj", "src/Novahome.Infrastructure/"]
RUN dotnet restore "src/Novahome.WebApi/Novahome.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Novahome.WebApi"
RUN dotnet build "Novahome.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Novahome.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Novahome.WebApi.dll"]
