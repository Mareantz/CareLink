# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# These ARGs allow for swapping out the base used to make the final image when debugging from VS
ARG LAUNCHING_FROM_VS
# This sets the base image for final, but only if LAUNCHING_FROM_VS has been defined
ARG FINAL_BASE_IMAGE=${LAUNCHING_FROM_VS:+aotdebug}

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# Install clang/zlib1g-dev dependencies for publishing to native
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    clang zlib1g-dev
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files (Updated paths to match folder structure)
COPY ["HealthcareManagementSystem/HealthcareManagementSystem/HealthcareManagementSystem.csproj", "HealthcareManagementSystem/"]
COPY ["HealthcareManagementSystem/Application/Application.csproj", "Application/"]
COPY ["HealthcareManagementSystem/Domain/Domain.csproj", "Domain/"]
COPY ["HealthcareManagementSystem/Infrastructure/Infrastructure.csproj", "Infrastructure/"]

# Restore dependencies
RUN dotnet restore "./HealthcareManagementSystem/HealthcareManagementSystem.csproj"

# Copy the full source code
COPY . .

WORKDIR "/src/HealthcareManagementSystem"

# Build the project
RUN dotnet build "./HealthcareManagementSystem/HealthcareManagementSystem.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release

# Publish as a self-contained app for Linux x64 architecture
RUN dotnet publish "./HealthcareManagementSystem/HealthcareManagementSystem.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --self-contained true \
    -r linux-x64 \                     # CHANGED: Ensure proper line continuation
    /p:PublishTrimmed=false            # CHANGED: Ensure no accidental assembly trimming

# Final stage: Use a minimal base image without relying on the system runtime
FROM debian:12-slim AS final           # CHANGED: Use Debian Slim as the base image
WORKDIR /app

# Add runtime dependencies required for self-contained apps
RUN apt-get update \
    && apt-get install -y --no-install-recommends libicu libssl3 zlib1g \
    && rm -rf /var/lib/apt/lists/*     # CHANGED: Install minimal runtime dependencies

EXPOSE 8080
EXPOSE 8081

# Copy published files from the publish stage
COPY --from=publish /app/publish .

# Set the entrypoint to the self-contained executable
ENTRYPOINT ["./HealthcareManagementSystem"] # CHANGED: No longer need "dotnet" since the app is self-contained
