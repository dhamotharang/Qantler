#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 56181
EXPOSE 56182

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY Services/HalalLibrary/HalalLibrary.API/HalalLibrary.API.csproj Services/HalalLibrary/HalalLibrary.API/
COPY Core/Core/Core.csproj Core/Core/
COPY Services/Core/Core.API/Core.API.csproj Services/Core/Core.API/
COPY Core/Core.EventBus/Core.EventBus.csproj Core/Core.EventBus/
COPY Services/HalalLibrary/HallalLibrary.Model/HalalLibrary.Model.csproj Services/HalalLibrary/HallalLibrary.Model/
COPY Services/Core/Core.Model/Core.Model.csproj Services/Core/Core.Model/
COPY Services/HalalLibrary/HalalLibrary.Events/HalalLibrary.Events.csproj Services/HalalLibrary/HalalLibrary.Events/
RUN dotnet restore "Services/HalalLibrary/HalalLibrary.API/HalalLibrary.API.csproj"
COPY . .
WORKDIR "/src/Services/HalalLibrary/HalalLibrary.API"
RUN dotnet build "HalalLibrary.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HalalLibrary.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HalalLibrary.API.dll"]
