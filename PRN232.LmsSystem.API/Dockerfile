FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PRN232.LmsSystem.API/PRN232.LmsSystem.API.csproj", "PRN232.LmsSystem.API/"]
COPY ["PRN232.LmsSystem.Services/PRN232.LmsSystem.Services.csproj", "PRN232.LmsSystem.Services/"]
COPY ["PRN232.LmsSystem.Repositories/PRN232.LmsSystem.Repositories.csproj", "PRN232.LmsSystem.Repositories/"]
RUN dotnet restore "./PRN232.LmsSystem.API/PRN232.LmsSystem.API.csproj"
COPY . .
WORKDIR "/src/PRN232.LmsSystem.API"
RUN dotnet build "./PRN232.LmsSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PRN232.LmsSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PRN232.LmsSystem.API.dll"]
