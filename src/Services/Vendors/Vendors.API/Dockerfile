FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /src

## Copy sln & csproj, restore as distinct layers
COPY "Services/Vendors/Vendors.sln" "Services/Vendors/Vendors.sln"

# Copy Shared projects
COPY "Services/Common/Common.csproj" "Services/Common/Common.csproj"
COPY "Services/Core/Core.csproj" "Services/Core/Core.csproj"

# Copy Vendors Service
COPY "Services/Vendors/Vendors.API/Vendors.API.csproj" "Services/Vendors/Vendors.API/Vendors.API.csproj"
COPY "Services/Vendors/Vendors.Application/Vendors.Application.csproj" "Services/Vendors/Vendors.Application/Vendors.Application.csproj"
COPY "Services/Vendors/Vendors.Infrastructure/Vendors.Infrastructure.csproj" "Services/Vendors/Vendors.Infrastructure/Vendors.Infrastructure.csproj"
COPY "Services/Vendors/Vendors.Domain/Vendors.Domain.csproj" "Services/Vendors/Vendors.Domain/Vendors.Domain.csproj"
COPY "Services/Vendors/Vendors.UnitTests/Vendors.UnitTests.csproj" "Services/Vendors/Vendors.UnitTests/Vendors.UnitTests.csproj"

#Copy Nuget config
COPY "nuget.config" "nuget.config"

# Restore Solutions
RUN dotnet restore "Services/Vendors/Vendors.sln"

## Handle Specific Project

# Copy everything else and build
COPY "Services/Common/" "Services/Common/"
COPY "Services/Core/" "Services/Core/"
COPY "Services/Vendors/" "Services/Vendors/"

WORKDIR /src/Services/Vendors/Vendors.API
RUN dotnet publish --no-restore -c Release -o out

FROM build-env as unittests
WORKDIR /src/Services/Vendors/Vendors.UnitTests

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /src/Services/Vendors/Vendors.API/out .
ENTRYPOINT ["dotnet", "Vendors.API.dll"]
