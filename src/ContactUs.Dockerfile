#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ContactUs/ContactUs.WebApi/ContactUs.WebApi.csproj", "ContactUs/ContactUs.WebApi/"]
COPY ["ContactUs/ContactUs.Application/ContactUs.Application.csproj", "ContactUs/ContactUs.Application/"]
COPY ["MessageBus/MessageBus.csproj", "MessageBus/"]
COPY ["ContactUs/ContactUs.Domain/ContactUs.Domain.csproj", "ContactUs/ContactUs.Domain/"]
COPY ["ContactUs/ContactUs.Infrastructure.Data/ContactUs.Infrastructure.Data.csproj", "ContactUs/ContactUs.Infrastructure.Data/"]
RUN dotnet restore "./ContactUs/ContactUs.WebApi/ContactUs.WebApi.csproj"
COPY . .
WORKDIR "/src/ContactUs/ContactUs.WebApi"
RUN dotnet build "./ContactUs.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ContactUs.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContactUs.WebApi.dll"]