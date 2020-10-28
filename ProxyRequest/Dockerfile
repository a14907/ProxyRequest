#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ProxyRequest/ProxyRequest.csproj", "ProxyRequest/"]
RUN dotnet restore "ProxyRequest/ProxyRequest.csproj"
COPY . .
WORKDIR "/src/ProxyRequest"
RUN dotnet build "ProxyRequest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProxyRequest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProxyRequest.dll"]