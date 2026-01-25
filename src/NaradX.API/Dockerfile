FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/NaradX.API/NaradX.API.csproj", "src/NaradX.API/"]
COPY ["src/NaradX.Business/NaradX.Business.csproj", "src/NaradX.Business/"]
COPY ["src/NaradX.Infrastructure/NaradX.Infrastructure.csproj", "src/NaradX.Infrastructure/"]
RUN dotnet restore "./src/NaradX.API/NaradX.API.csproj"
COPY . .
WORKDIR "/src/src/NaradX.API"
RUN dotnet build "./NaradX.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NaradX.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NaradX.API.dll"]
