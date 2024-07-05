#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MigraineBuddyConvertor/MigraineBuddyConvertor.csproj", "MigraineBuddyConvertor/"]
RUN dotnet restore "./MigraineBuddyConvertor/MigraineBuddyConvertor.csproj"
COPY . .
WORKDIR "/src/MigraineBuddyConvertor"
RUN dotnet build "./MigraineBuddyConvertor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MigraineBuddyConvertor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MigraineBuddyConvertor.dll"]

COPY ./bin/Debug/net8.0/wwwroot/fonts/times.ttf /usr/share/fonts/truetype/
COPY ./bin/Debug/net8.0/wwwroot/fonts/timesbd.ttf /usr/share/fonts/truetype/
COPY ./bin/Debug/net8.0/wwwroot/fonts/timesbi.ttf /usr/share/fonts/truetype/
COPY ./bin/Debug/net8.0/wwwroot/fonts/timesi.ttf /usr/share/fonts/truetype/
