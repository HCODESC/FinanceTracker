# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FinanceTracker.API/FinanceTracker.API.csproj", "FinanceTracker.API/"]
COPY ["FinanceTracker.Shared/FinanceTracker.Shared.csproj", "FinanceTracker.Shared/"]
RUN dotnet restore "./FinanceTracker.API/FinanceTracker.API.csproj"
COPY . .
WORKDIR "/src/FinanceTracker.API"
RUN dotnet build "./FinanceTracker.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FinanceTracker.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinanceTracker.API.dll"]
