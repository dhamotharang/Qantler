#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY Services/File/File.API/File.API.csproj Services/File/File.API/
COPY Core/Core.EventBus/Core.EventBus.csproj Core/Core.EventBus/
COPY Core/Core/Core.csproj Core/Core/
COPY Services/Core/Core.API/Core.API.csproj Services/Core/Core.API/
COPY Services/File/File.Model/File.Model.csproj Services/File/File.Model/
RUN dotnet restore "Services/File/File.API/File.API.csproj"
COPY . .
WORKDIR "/src/Services/File/File.API"
RUN dotnet build "File.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "File.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "File.API.dll"]
