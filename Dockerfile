FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS dotnet-runtime

# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
RUN apt-get update && apt-get install -y ca-certificates gnupg
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY SpellWork/*.csproj ./SpellWork/
COPY SpellWork.Common/*.csproj ./SpellWork.Common/
RUN dotnet restore -r linux-x64 SpellWork/

# copy everything else and build app
COPY SpellWork/. ./SpellWork/
COPY SpellWork.Common/. ./SpellWork.Common/
WORKDIR /source/SpellWork
RUN dotnet publish SpellWork.csproj -c release -o /app -r linux-x64 --self-contained false --no-restore /p:DebugType=Embedded

# final stage/image
FROM dotnet-runtime
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["./SpellWork"]
