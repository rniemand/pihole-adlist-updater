FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

RUN apt-get update
RUN apt-get install -y wget cmake python build-essential

RUN wget https://github.com/libgit2/libgit2/archive/v0.24.0.tar.gz && \
tar xzf v0.24.0.tar.gz && \
cd libgit2-0.24.0/ && \
cmake . && \
make && \
make install

COPY ["src/PiHoleUpdater.Common/PiHoleUpdater.Common.csproj", "PiHoleUpdater.Common/"]
COPY ["src/PiHoleUpdaterDocker/PiHoleUpdaterDocker.csproj", "PiHoleUpdaterDocker/"]

RUN dotnet restore "PiHoleUpdaterDocker/PiHoleUpdaterDocker.csproj"

COPY ["src/PiHoleUpdater.Common/", "PiHoleUpdater.Common/"]
COPY ["src/PiHoleUpdaterDocker/", "PiHoleUpdaterDocker/"]

WORKDIR "/src/PiHoleUpdaterDocker"

RUN dotnet build "PiHoleUpdaterDocker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PiHoleUpdaterDocker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PiHoleUpdaterDocker.dll"]
