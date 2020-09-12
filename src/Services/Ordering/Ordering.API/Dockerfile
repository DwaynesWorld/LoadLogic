FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /src

## Copy sln & csproj, restore as distinct layers
COPY "Services/Ordering/Ordering.sln" "Services/Ordering/Ordering.sln"

# Copy Shared projects
COPY "Services/Common/Common.csproj" "Services/Common/Common.csproj"
COPY "Services/Core/Core.csproj" "Services/Core/Core.csproj"

# Copy Ordering Service
COPY "Services/Ordering/Ordering.API/Ordering.API.csproj" "Services/Ordering/Ordering.API/Ordering.API.csproj"
COPY "Services/Ordering/Ordering.Application/Ordering.Application.csproj" "Services/Ordering/Ordering.Application/Ordering.Application.csproj"
COPY "Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj" "Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj"
COPY "Services/Ordering/Ordering.Domain/Ordering.Domain.csproj" "Services/Ordering/Ordering.Domain/Ordering.Domain.csproj"
COPY "Services/Ordering/Ordering.UnitTests/Ordering.UnitTests.csproj" "Services/Ordering/Ordering.UnitTests/Ordering.UnitTests.csproj"

#Copy Nuget config
COPY "nuget.config" "nuget.config"

# Restore Solutions
RUN dotnet restore "Services/Ordering/Ordering.sln"

## Handle Specific Project

# Copy everything else and build
COPY "Services/Common/" "Services/Common/"
COPY "Services/Core/" "Services/Core/"
COPY "Services/Ordering/" "Services/Ordering/"

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
