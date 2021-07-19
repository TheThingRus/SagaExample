#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
#RUN apt-get update && apt-get install -y iputils-ping
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SagaExample.IdentityServer/SagaExample.IdentityServer.csproj", "SagaExample.IdentityServer/"]
RUN dotnet restore "SagaExample.IdentityServer/SagaExample.IdentityServer.csproj"
COPY . .
WORKDIR "/src/SagaExample.IdentityServer"
RUN dotnet build "SagaExample.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SagaExample.IdentityServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SagaExample.IdentityServer.dll"]
