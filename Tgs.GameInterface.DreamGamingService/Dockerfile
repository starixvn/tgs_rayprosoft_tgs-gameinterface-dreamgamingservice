#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameInterface.DreamGamingService/Tgs.GameInterface.DreamGamingService.csproj", "Tgs.GameInterface.DreamGamingService/"]
COPY ["Tgs.GameInterface.DreamGamingService.Core/Tgs.GameInterface.DreamGamingService.Core.csproj", "Tgs.GameInterface.DreamGamingService.Core/"]
COPY ["Tgs.GameInterface.DreamGamingService.Protos/Tgs.GameInterface.DreamGamingService.Protos.csproj", "Tgs.GameInterface.DreamGamingService.Protos/"]
RUN dotnet restore "./Tgs.GameInterface.DreamGamingService/Tgs.GameInterface.DreamGamingService.csproj"
COPY . .
WORKDIR "/src/Tgs.GameInterface.DreamGamingService"
RUN dotnet build "./Tgs.GameInterface.DreamGamingService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.GameInterface.DreamGamingService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameInterface.DreamGamingService.dll"]