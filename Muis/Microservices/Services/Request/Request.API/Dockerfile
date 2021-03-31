#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 56121
EXPOSE 56122

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY Services/Request/Request.API/Request.API.csproj Services/Request/Request.API/
COPY Services/Core/Core.API/Core.API.csproj Services/Core/Core.API/
COPY Core/Core/Core.csproj Core/Core/
COPY Services/Request/Request.Events/Request.Events.csproj Services/Request/Request.Events/
COPY Core/Core.EventBus/Core.EventBus.csproj Core/Core.EventBus/
COPY Services/Request/Request.Model/Request.Model.csproj Services/Request/Request.Model/
COPY Services/Core/Core.Model/Core.Model.csproj Services/Core/Core.Model/
RUN dotnet restore "Services/Request/Request.API/Request.API.csproj"
COPY . .
WORKDIR "/src/Services/Request/Request.API"
RUN dotnet build "Request.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Request.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Request.API.dll"]
