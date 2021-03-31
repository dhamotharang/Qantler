#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY eHS-Portal/eHS-Portal.csproj eHS-Portal/
COPY eHS-Portal.Model/eHS-Portal.Model.csproj eHS-Portal.Model/
COPY Core/Core.EventBus/Core.EventBus.csproj Core/Core.EventBus/
COPY Core/Core/Core.csproj Core/Core/
RUN dotnet restore "eHS-Portal/eHS-Portal.csproj"
COPY . .
WORKDIR "/src/eHS-Portal"
RUN dotnet build "eHS-Portal.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eHS-Portal.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eHS-Portal.dll"]
