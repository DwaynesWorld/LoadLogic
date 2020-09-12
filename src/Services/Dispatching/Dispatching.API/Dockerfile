FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /src

## Copy sln & csproj, restore as distinct layers
COPY "Services/Dispatching/Dispatching.sln" "Services/Dispatching/Dispatching.sln"

# Copy Shared projects
COPY "Services/Common/Common.csproj" "Services/Common/Common.csproj"
COPY "Services/Core/Core.csproj" "Services/Core/Core.csproj"

# Copy Dispatching Service
COPY "Services/Dispatching/Dispatching.API/Dispatching.API.csproj" "Services/Dispatching/Dispatching.API/Dispatching.API.csproj"
COPY "Services/Dispatching/Dispatching.Application/Dispatching.Application.csproj" "Services/Dispatching/Dispatching.Application/Dispatching.Application.csproj"
COPY "Services/Dispatching/Dispatching.Infrastructure/Dispatching.Infrastructure.csproj" "Services/Dispatching/Dispatching.Infrastructure/Dispatching.Infrastructure.csproj"
COPY "Services/Dispatching/Dispatching.Domain/Dispatching.Domain.csproj" "Services/Dispatching/Dispatching.Domain/Dispatching.Domain.csproj"
COPY "Services/Dispatching/Dispatching.UnitTests/Dispatching.UnitTests.csproj" "Services/Dispatching/Dispatching.UnitTests/Dispatching.UnitTests.csproj"

#Copy Nuget config
COPY "nuget.config" "nuget.config"

# Restore Solutions
RUN dotnet restore "Services/Dispatching/Dispatching.sln"

## Handle Specific Project

# Copy everything else and build
COPY "Services/Common/" "Services/Common/"
COPY "Services/Core/" "Services/Core/"
COPY "Services/Dispatching/" "Services/Dispatching/"

WORKDIR /src/Services/Dispatching/Dispatching.API
RUN dotnet publish --no-restore -c Release -o out

FROM build-env as unittests
WORKDIR /src/Services/Dispatching/Dispatching.UnitTests

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /src/Services/Dispatching/Dispatching.API/out .
ENTRYPOINT ["dotnet", "Dispatching.API.dll"]
