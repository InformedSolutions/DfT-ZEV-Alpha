#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build

COPY stylecop.ruleset stylecop.ruleset

WORKDIR /src
COPY ["src/services/apis/scheme-data/DfT.ZEV.Services.SchemeData.Api/DfT.ZEV.Services.SchemeData.Api.csproj", "./services/apis/scheme-data/DfT.ZEV.Services.SchemeData.Api/"]

COPY ["src/core/DfT.ZEV.Core.Application/DfT.ZEV.Core.Application.csproj", "./core/DfT.ZEV.Core.Application/"]
COPY ["src/core/DfT.ZEV.Core.Infrastructure/DfT.ZEV.Core.Infrastructure.csproj", "./core/DfT.ZEV.Core.Infrastructure/"]
COPY ["src/core/DfT.ZEV.Core.Domain/DfT.ZEV.Core.Domain.csproj", "./core/DfT.ZEV.Core.Domain/"]

COPY ["src/common/DfT.ZEV.Common/DfT.ZEV.Common.csproj", "./common/DfT.ZEV.Common/"]
COPY ["src/common/DfT.ZEV.Common.MVC.Authentication/DfT.ZEV.Common.MVC.Authentication.csproj", "./common/DfT.ZEV.Common.MVC.Authentication/"]

WORKDIR /src/services/apis/scheme-data/DfT.ZEV.Services.SchemeData.Api
COPY src/Directory.Packages.props .
RUN dotnet restore

WORKDIR /src
COPY src/. .

FROM build AS publish
WORKDIR /src/services/apis/scheme-data/DfT.ZEV.Services.SchemeData.Api
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
RUN apk add --no-cache tzdata
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./DfT.ZEV.Services.SchemeData.Api"]