FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /src

## Copy sln & csproj, restore as distinct layers
COPY "LoadLogic.sln" "LoadLogic.sln"

# Copy Shared projects
COPY "Services/Common/Common.csproj" "Services/Common/Common.csproj"
COPY "Services/Core/Core.csproj" "Services/Core/Core.csproj"

# Copy Dispatching Service
COPY "Services/Dispatching/Dispatching.API/Dispatching.API.csproj" "Services/Dispatching/Dispatching.API/Dispatching.API.csproj"
COPY "Services/Dispatching/Dispatching.Application/Dispatching.Application.csproj" "Services/Dispatching/Dispatching.Application/Dispatching.Application.csproj"
COPY "Services/Dispatching/Dispatching.Infrastructure/Dispatching.Infrastructure.csproj" "Services/Dispatching/Dispatching.Infrastructure/Dispatching.Infrastructure.csproj"
COPY "Services/Dispatching/Dispatching.Domain/Dispatching.Domain.csproj" "Services/Dispatching/Dispatching.Domain/Dispatching.Domain.csproj"
COPY "Services/Dispatching/Dispatching.UnitTests/Dispatching.UnitTests.csproj" "Services/Dispatching/Dispatching.UnitTests/Dispatching.UnitTests.csproj"

# Copy Hauling Service
COPY "Services/Hauling/Hauling.API/Hauling.API.csproj" "Services/Hauling/Hauling.API/Hauling.API.csproj"
COPY "Services/Hauling/Hauling.Application/Hauling.Application.csproj" "Services/Hauling/Hauling.Application/Hauling.Application.csproj"
COPY "Services/Hauling/Hauling.Infrastructure/Hauling.Infrastructure.csproj" "Services/Hauling/Hauling.Infrastructure/Hauling.Infrastructure.csproj"
COPY "Services/Hauling/Hauling.Domain/Hauling.Domain.csproj" "Services/Hauling/Hauling.Domain/Hauling.Domain.csproj"
COPY "Services/Hauling/Hauling.UnitTests/Hauling.UnitTests.csproj" "Services/Hauling/Hauling.UnitTests/Hauling.UnitTests.csproj"

# Copy Ordering Service
COPY "Services/Ordering/Ordering.API/Ordering.API.csproj" "Services/Ordering/Ordering.API/Ordering.API.csproj"
COPY "Services/Ordering/Ordering.Application/Ordering.Application.csproj" "Services/Ordering/Ordering.Application/Ordering.Application.csproj"
COPY "Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj" "Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj"
COPY "Services/Ordering/Ordering.Domain/Ordering.Domain.csproj" "Services/Ordering/Ordering.Domain/Ordering.Domain.csproj"
COPY "Services/Ordering/Ordering.UnitTests/Ordering.UnitTests.csproj" "Services/Ordering/Ordering.UnitTests/Ordering.UnitTests.csproj"

# Copy Ordering Service
COPY "Services/Ordering/Ordering.API/Ordering.API.csproj" "Services/Ordering/Ordering.API/Ordering.API.csproj"
COPY "Services/Ordering/Ordering.Application/Ordering.Application.csproj" "Services/Ordering/Ordering.Application/Ordering.Application.csproj"
COPY "Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj" "Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj"
COPY "Services/Ordering/Ordering.Domain/Ordering.Domain.csproj" "Services/Ordering/Ordering.Domain/Ordering.Domain.csproj"
COPY "Services/Ordering/Ordering.UnitTests/Ordering.UnitTests.csproj" "Services/Ordering/Ordering.UnitTests/Ordering.UnitTests.csproj"

# Copy Payments Service
COPY "Services/Payments/Payments.API/Payments.API.csproj" "Services/Payments/Payments.API/Payments.API.csproj"
COPY "Services/Payments/Payments.Application/Payments.Application.csproj" "Services/Payments/Payments.Application/Payments.Application.csproj"
COPY "Services/Payments/Payments.Infrastructure/Payments.Infrastructure.csproj" "Services/Payments/Payments.Infrastructure/Payments.Infrastructure.csproj"
COPY "Services/Payments/Payments.Domain/Payments.Domain.csproj" "Services/Payments/Payments.Domain/Payments.Domain.csproj"
COPY "Services/Payments/Payments.UnitTests/Payments.UnitTests.csproj" "Services/Payments/Payments.UnitTests/Payments.UnitTests.csproj"

# Copy Vendors Service
COPY "Services/Vendors/Vendors.API/Vendors.API.csproj" "Services/Vendors/Vendors.API/Vendors.API.csproj"
COPY "Services/Vendors/Vendors.Application/Vendors.Application.csproj" "Services/Vendors/Vendors.Application/Vendors.Application.csproj"
COPY "Services/Vendors/Vendors.Infrastructure/Vendors.Infrastructure.csproj" "Services/Vendors/Vendors.Infrastructure/Vendors.Infrastructure.csproj"
COPY "Services/Vendors/Vendors.Domain/Vendors.Domain.csproj" "Services/Vendors/Vendors.Domain/Vendors.Domain.csproj"
COPY "Services/Vendors/Vendors.UnitTests/Vendors.UnitTests.csproj" "Services/Vendors/Vendors.UnitTests/Vendors.UnitTests.csproj"

#Copy Nuget config
COPY "nuget.config" "nuget.config"

# Restore Solutions
RUN dotnet restore "LoadLogic.sln"

## Handle Specific Project

# Copy everything else and build
COPY . .
WORKDIR /src/Services/Ordering/Ordering.API
RUN dotnet publish --no-restore -c Release -o out

FROM build-env as unittests
WORKDIR /src/Services/Ordering/Ordering.UnitTests

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /src/Services/Ordering/Ordering.API/out .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]
