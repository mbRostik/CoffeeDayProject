# Use the appropriate .NET image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["MenuService/Menu.WebApi/Menu.WebApi.csproj", "MenuService/Menu.WebApi/"]
COPY ["MenuService/Menu.Application/Menu.Application.csproj", "MenuService/Menu.Application/"]
COPY ["MessageBus/MessageBus.csproj", "MessageBus/"]
COPY ["MenuService/Menu.Domain/Menu.Domain.csproj", "MenuService/Menu.Domain/"]
COPY ["MenuService/Menu.Infrastructure.Data/Menu.Infrastructure.Data.csproj", "MenuService/Menu.Infrastructure.Data/"]
RUN dotnet restore "./MenuService/Menu.WebApi/Menu.WebApi.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/MenuService/Menu.WebApi"
RUN dotnet build "./Menu.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Menu.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage / image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy the images directory
COPY MenuService/Menu.WebApi/bin/Debug/net8.0/images /app/images

ENTRYPOINT ["dotnet", "Menu.WebApi.dll"]
