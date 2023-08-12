#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BestStories.Api/BestStories.Api.csproj", "BestStories.Api/"]
RUN dotnet restore "BestStories.Api/BestStories.Api.csproj"
COPY . .
WORKDIR "/src/BestStories.Api"
RUN dotnet build "BestStories.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BestStories.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BestStories.Api.dll"]

RUN apt-get update && apt-get install -y git
