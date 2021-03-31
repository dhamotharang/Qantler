#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY Services/JobOrder/JobOrder.API/JobOrder.API.csproj Services/JobOrder/JobOrder.API/
COPY Services/Core/Core.API/Core.API.csproj Services/Core/Core.API/
COPY Core/Core/Core.csproj Core/Core/
COPY Services/Request/Request.Model/Request.Model.csproj Services/Request/Request.Model/
COPY Services/Core/Core.Model/Core.Model.csproj Services/Core/Core.Model/
COPY Core/Core.EventBus/Core.EventBus.csproj Core/Core.EventBus/
COPY Services/JobOrder/JobOrder.Model/JobOrder.Model.csproj Services/JobOrder/JobOrder.Model/
RUN dotnet restore "Services/JobOrder/JobOrder.API/JobOrder.API.csproj"
COPY . .
WORKDIR "/src/Services/JobOrder/JobOrder.API"
RUN dotnet build "JobOrder.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JobOrder.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobOrder.API.dll"]
