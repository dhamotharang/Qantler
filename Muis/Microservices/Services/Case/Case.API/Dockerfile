#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 56171
EXPOSE 56172

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY Services/Case/Case.API/Case.API.csproj Services/Case/Case.API/
COPY Services/Case/Case.Model/Case.Model.csproj Services/Case/Case.Model/
COPY Services/Core/Core.Model/Core.Model.csproj Services/Core/Core.Model/
COPY Services/Core/Core.API/Core.API.csproj Services/Core/Core.API/
COPY Core/Core/Core.csproj Core/Core/
COPY Core/Core.EventBus/Core.EventBus.csproj Core/Core.EventBus/
COPY Services/Case/Case.Events/Case.Events.csproj Services/Case/Case.Events/
RUN dotnet restore "Services/Case/Case.API/Case.API.csproj"
COPY . .
WORKDIR "/src/Services/Case/Case.API"
RUN dotnet build "Case.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Case.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Case.API.dll"]
