# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# These ARGs allow for swapping out the base used to make the final image when debugging from VS
ARG LAUNCHING_FROM_VS
# This sets the base image for final, but only if LAUNCHING_FROM_VS has been defined
ARG FINAL_BASE_IMAGE=${LAUNCHING_FROM_VS:+aotdebug}

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["HealthcareManagementSystem/HealthcareManagementSystem/HealthcareManagementSystem.csproj", "HealthcareManagementSystem/"]
COPY ["HealthcareManagementSystem/Application/Application.csproj", "Application/"]
COPY ["HealthcareManagementSystem/Domain/Domain.csproj", "Domain/"]
COPY ["HealthcareManagementSystem/Infrastructure/Infrastructure.csproj", "Infrastructure/"]

# Restore dependencies
RUN dotnet restore "./HealthcareManagementSystem/HealthcareManagementSystem.csproj"

# Copy the rest of the source code
COPY . .

WORKDIR "/src/HealthcareManagementSystem"

# Build the project
RUN dotnet build "./HealthcareManagementSystem/HealthcareManagementSystem.csproj" -c Release -o /app/build

# Publish the application as self-contained
RUN dotnet publish "./HealthcareManagementSystem/HealthcareManagementSystem.csproj" \
    -c Release \
    -o /app/publish \
    --self-contained true # CHANGED: Include the .NET runtime

# Final stage: Use a bare base image
FROM mcr.microsoft.com/dotnet/runtime-deps:9.0 AS final
WORKDIR /app

# Copy published files from the build stage
COPY --from=build /app/publish .

# Expose the necessary ports
EXPOSE 8080
EXPOSE 8081

# Set the entrypoint to the self-contained executable
ENTRYPOINT ["./HealthcareManagementSystem"] # CHANGED: Run the app directly since it's self-contained
