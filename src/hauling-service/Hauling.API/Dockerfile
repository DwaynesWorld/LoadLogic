FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /src

## Copy sln & csproj, restore as distinct layers
COPY "Services/Hauling/Hauling.sln" "Services/Hauling/Hauling.sln"

# Copy Shared projects
COPY "Services/Common/Common.csproj" "Services/Common/Common.csproj"
COPY "Services/Core/Core.csproj" "Services/Core/Core.csproj"

# Copy Hauling Service
COPY "Services/Hauling/Hauling.API/Hauling.API.csproj" "Services/Hauling/Hauling.API/Hauling.API.csproj"
COPY "Services/Hauling/Hauling.Application/Hauling.Application.csproj" "Services/Hauling/Hauling.Application/Hauling.Application.csproj"
COPY "Services/Hauling/Hauling.Infrastructure/Hauling.Infrastructure.csproj" "Services/Hauling/Hauling.Infrastructure/Hauling.Infrastructure.csproj"
COPY "Services/Hauling/Hauling.Domain/Hauling.Domain.csproj" "Services/Hauling/Hauling.Domain/Hauling.Domain.csproj"
COPY "Services/Hauling/Hauling.UnitTests/Hauling.UnitTests.csproj" "Services/Hauling/Hauling.UnitTests/Hauling.UnitTests.csproj"

#Copy Nuget config
COPY "nuget.config" "nuget.config"

# Restore Solutions
RUN dotnet restore "Services/Hauling/Hauling.sln"

## Handle Specific Project

# Copy everything else and build
COPY "Services/Common/" "Services/Common/"
COPY "Services/Core/" "Services/Core/"
COPY "Services/Hauling/" "Services/Hauling/"

WORKDIR /src/Services/Hauling/Hauling.API
RUN dotnet publish --no-restore -c Release -o out

FROM build-env as unittests
WORKDIR /src/Services/Hauling/Hauling.UnitTests

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /src/Services/Hauling/Hauling.API/out .
ENTRYPOINT ["dotnet", "Hauling.API.dll"]
