FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /src

## Copy sln & csproj, restore as distinct layers
COPY "Services/Payments/Payments.sln" "Services/Payments/Payments.sln"

# Copy Shared projects
COPY "Services/Common/Common.csproj" "Services/Common/Common.csproj"
COPY "Services/Core/Core.csproj" "Services/Core/Core.csproj"

# Copy Payments Service
COPY "Services/Payments/Payments.API/Payments.API.csproj" "Services/Payments/Payments.API/Payments.API.csproj"
COPY "Services/Payments/Payments.Application/Payments.Application.csproj" "Services/Payments/Payments.Application/Payments.Application.csproj"
COPY "Services/Payments/Payments.Infrastructure/Payments.Infrastructure.csproj" "Services/Payments/Payments.Infrastructure/Payments.Infrastructure.csproj"
COPY "Services/Payments/Payments.Domain/Payments.Domain.csproj" "Services/Payments/Payments.Domain/Payments.Domain.csproj"
COPY "Services/Payments/Payments.UnitTests/Payments.UnitTests.csproj" "Services/Payments/Payments.UnitTests/Payments.UnitTests.csproj"

#Copy Nuget config
COPY "nuget.config" "nuget.config"

# Restore Solutions
RUN dotnet restore "Services/Payments/Payments.sln"

## Handle Specific Project

# Copy everything else and build
COPY "Services/Common/" "Services/Common/"
COPY "Services/Core/" "Services/Core/"
COPY "Services/Payments/" "Services/Payments/"

WORKDIR /src/Services/Payments/Payments.API
RUN dotnet publish --no-restore -c Release -o out

FROM build-env as unittests
WORKDIR /src/Services/Payments/Payments.UnitTests

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /src/Services/Payments/Payments.API/out .
ENTRYPOINT ["dotnet", "Payments.API.dll"]
