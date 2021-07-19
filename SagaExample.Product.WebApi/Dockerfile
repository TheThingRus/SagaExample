#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["SagaExample.Product.WebApi/SagaExample.Product.WebApi.csproj", "SagaExample.Product.WebApi/"]
RUN dotnet restore "SagaExample.Product.WebApi/SagaExample.Product.WebApi.csproj"
COPY . .
WORKDIR "/src/SagaExample.Product.WebApi"
RUN dotnet build "SagaExample.Product.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SagaExample.Product.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SagaExample.Product.WebApi.dll"]