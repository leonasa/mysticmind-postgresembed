#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

# Add the PostgreSQL PGP key to verify their Debian packages.
# It should be the same key as https://www.postgresql.org/media/keys/ACCC4CF8.asc
RUN apt-key adv --keyserver hkp://p80.pool.sks-keyservers.net:80 --recv-keys B97B0AFCAA1A47F044F244A07FCC7D46ACCC4CF8

# Add PostgreSQL's repository. It contains the most recent stable release
#  of PostgreSQL.
RUN echo "deb http://apt.postgresql.org/pub/repos/apt/ precise-pgdg main" > /etc/apt/sources.list.d/pgdg.list

# Install ``python-software-properties``, ``software-properties-common`` and PostgreSQL 11
#  There are some warnings (in red) that show up during the build. You can hide
#  them by prefixing each apt-get statement with DEBIAN_FRONTEND=noninteractive
RUN apt-get update 
RUN apt-get install -y postgresql-11

WORKDIR /src
COPY ["MysticMind.PostgresEmbed.Runnable/MysticMind.PostgresEmbed.Runnable.csproj", "MysticMind.PostgresEmbed.Runnable/"]
RUN dotnet restore "MysticMind.PostgresEmbed.Runnable/MysticMind.PostgresEmbed.Runnable.csproj"
COPY . .
WORKDIR "/src/MysticMind.PostgresEmbed.Runnable"
RUN dotnet build "MysticMind.PostgresEmbed.Runnable.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MysticMind.PostgresEmbed.Runnable.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MysticMind.PostgresEmbed.Runnable.dll"]