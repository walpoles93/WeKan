#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/WeKan.API/WeKan.API.csproj", "src/WeKan.API/"]
RUN dotnet restore "src/WeKan.API/WeKan.API.csproj"
COPY . .
WORKDIR "/src/src/WeKan.API"
RUN dotnet build "WeKan.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeKan.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeKan.API.dll"]