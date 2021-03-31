#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 56161
EXPOSE 56162

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY Services/Finance/Finance.API/Finance.API.csproj Services/Finance/Finance.API/
COPY Core/Core/Core.csproj Core/Core/
COPY Services/Core/Core.API/Core.API.csproj Services/Core/Core.API/
COPY Core/Core.EventBus/Core.EventBus.csproj Core/Core.EventBus/
COPY Services/Core/Core.Model/Core.Model.csproj Services/Core/Core.Model/
COPY Services/Finance/Finance.Model/Finance.Model.csproj Services/Finance/Finance.Model/
RUN dotnet restore "Services/Finance/Finance.API/Finance.API.csproj"
COPY . .
WORKDIR "/src/Services/Finance/Finance.API"
RUN dotnet build "Finance.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Finance.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Finance.API.dll"]
