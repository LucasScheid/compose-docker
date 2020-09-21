#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["api-read.service/api-read.service.csproj", "api-read.service/"]
RUN dotnet restore "api-read.service/api-read.service.csproj"
COPY . .
WORKDIR "/src/api-read.service"
RUN dotnet build "api-read.service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "api-read.service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api-read.service.dll"]