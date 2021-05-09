FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /src

COPY "services/ordering/Ordering.sln" "services/ordering/Ordering.sln"
COPY "common/Common.csproj" "common/Common.csproj"
COPY "services/ordering/Ordering/Ordering.csproj" "services/ordering/Ordering/Ordering.csproj"
COPY "services/ordering/Ordering.UnitTests/Ordering.UnitTests.csproj" "services/ordering/Ordering.UnitTests/Ordering.UnitTests.csproj"
COPY "nuget.config" "nuget.config"

# Restore Solutions
RUN dotnet restore "services/ordering/Ordering.sln"

# Copy everything else and build
COPY "common/" "common/"
COPY "services/ordering/" "services/ordering/"

WORKDIR /src/services/ordering/Ordering
RUN dotnet publish --no-restore -c Release -o out

FROM build-env as unittests
WORKDIR /src/services/ordering/Ordering.UnitTests

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /src/services/ordering/Ordering/out .
ENTRYPOINT ["dotnet", "Ordering.dll"]
