FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /src

COPY "ordering/Ordering.sln" "ordering/Ordering.sln"
COPY "common/Common.csproj" "common/Common.csproj"
COPY "ordering/Ordering/Ordering.csproj" "ordering/Ordering/Ordering.csproj"
COPY "ordering/Ordering.UnitTests/Ordering.UnitTests.csproj" "ordering/Ordering.UnitTests/Ordering.UnitTests.csproj"
COPY "nuget.config" "nuget.config"

# Restore Solutions
RUN dotnet restore "ordering/Ordering.sln"

# Copy everything else and build
COPY "common/" "common/"
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
