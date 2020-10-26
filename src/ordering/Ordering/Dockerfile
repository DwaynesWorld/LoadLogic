FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /src

## Copy sln & csproj, restore as distinct layers
COPY "ordering/Ordering.sln" "ordering/Ordering.sln"

# Copy Shared projects
COPY "common/Common.csproj" "common/Common.csproj"
COPY "core/Core.csproj" "core/Core.csproj"

# Copy Ordering Service
COPY "ordering/Ordering/Ordering.csproj" "ordering/Ordering/Ordering.csproj"
COPY "ordering/Ordering.UnitTests/Ordering.UnitTests.csproj" "ordering/Ordering.UnitTests/Ordering.UnitTests.csproj"

#Copy Nuget config
COPY "nuget.config" "nuget.config"

# Restore Solutions
RUN dotnet restore "ordering/Ordering.sln"

## Handle Specific Project

# Copy everything else and build
COPY "common/" "common/"
COPY "core/" "core/"
COPY "ordering/" "ordering/"

WORKDIR /src/ordering/Ordering
RUN dotnet publish --no-restore -c Release -o out

FROM build-env as unittests
WORKDIR /src/ordering/Ordering.UnitTests

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /src/ordering/Ordering/out .
ENTRYPOINT ["dotnet", "Ordering.dll"]
