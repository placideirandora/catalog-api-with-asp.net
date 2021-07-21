# STAGE 1 - PREPARE THE RUNTIME FOR PROJECT
FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80

# STAGE 2 - BUILD THE PROJECT
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["Catalog.Api/Catalog.Api.csproj", "Catalog.Api/"]
RUN dotnet restore "Catalog.Api/Catalog.Api.csproj"
COPY ["Catalog.Api/", "Catalog.Api/"]
WORKDIR /src/Catalog.Api
RUN dotnet build "Catalog.Api.csproj" -c Release --no-restore

# test stage -- exposes optional entrypoint
# target entrypoint with: docker build --target test

# STAGE 3 - TEST THE PROJECT
FROM build AS test
WORKDIR /src/tests
COPY Catalog.UnitTests/ .
ENTRYPOINT ["dotnet", "test", "--logger:trx;"]

# STAGE 4 - PUBLISH THE PROJECT
FROM build AS publish
RUN dotnet publish "Catalog.Api.csproj" -c Release --no-build -o /app/publish

# STAGE 5 - RUN THE PUBLISHED PROJECT
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]
