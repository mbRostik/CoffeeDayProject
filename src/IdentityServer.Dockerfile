FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["IdentityServer/IdentityServerAPI/IdentityServerAPI.csproj", "IdentityServer/IdentityServerAPI/"]
COPY ["MessageBus/MessageBus.csproj", "MessageBus/"]
COPY ["IdentityServer/IdentityServer.DAL/IdentityServer.DAL.csproj", "IdentityServer/IdentityServer.DAL/"]
RUN dotnet restore "./IdentityServer/IdentityServerAPI/IdentityServerAPI.csproj"
COPY . .
WORKDIR "/src/IdentityServer/IdentityServerAPI"
RUN dotnet build "./IdentityServerAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IdentityServerAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY ["certs/identityserverapi-api.pfx", "https/"]
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityServerAPI.dll"]