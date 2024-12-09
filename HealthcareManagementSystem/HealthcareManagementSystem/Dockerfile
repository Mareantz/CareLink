# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/HealthcareManagementSystem
COPY ["HealthcareManagementSystem/HealthcareManagementSystem.csproj", "HealthcareManagementSystem/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Identity/Identity.csproj", "Identity/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "./HealthcareManagementSystem/HealthcareManagementSystem.csproj"
COPY . .
WORKDIR "/src/HealthcareManagementSystem"
RUN dotnet build "./HealthcareManagementSystem/HealthcareManagementSystem.csproj" -c Release -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HealthcareManagementSystem/HealthcareManagementSystem.csproj" \
    -c Release \
    -o /app/publish \
    --self-contained true

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
EXPOSE 8081
ENTRYPOINT ["./HealthcareManagementSystem"] # CHANGED: Run the app directly since it's self-contained
