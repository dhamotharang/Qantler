#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY Services/Identity/Identity.API/Identity.API.csproj Services/Identity/Identity.API/
COPY Services/Identity/Identity.Model/Identity.Model.csproj Services/Identity/Identity.Model/
COPY Services/Core/Core.Model/Core.Model.csproj Services/Core/Core.Model/
COPY Services/Core/Core.API/Core.API.csproj Services/Core/Core.API/
COPY Core/Core/Core.csproj Core/Core/
COPY Core/Core.EventBus/Core.EventBus.csproj Core/Core.EventBus/
RUN dotnet restore "Services/Identity/Identity.API/Identity.API.csproj"
COPY . .
WORKDIR "/src/Services/Identity/Identity.API"
RUN dotnet build "Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.API.dll"]
