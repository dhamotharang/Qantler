#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY Services/Notification/Notification.API/Notification.API.csproj Services/Notification/Notification.API/
COPY Services/Notification/Notification.Events/Notification.Events.csproj Services/Notification/Notification.Events/
COPY Core/Core.EventBus/Core.EventBus.csproj Core/Core.EventBus/
COPY Services/Notification/Notification.Model/Notification.Model.csproj Services/Notification/Notification.Model/
COPY Services/Core/Core.API/Core.API.csproj Services/Core/Core.API/
COPY Core/Core/Core.csproj Core/Core/
RUN dotnet restore "Services/Notification/Notification.API/Notification.API.csproj"
COPY . .
WORKDIR "/src/Services/Notification/Notification.API"
RUN dotnet build "Notification.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Notification.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notification.API.dll"]
