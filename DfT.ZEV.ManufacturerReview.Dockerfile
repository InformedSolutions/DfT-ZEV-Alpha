#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build

WORKDIR /src
COPY ["src/services/websites/manufacturer-data-review-portal/DfT.ZEV.ManufacturerReview.Web/DfT.ZEV.ManufacturerReview.Web.csproj", "./services/websites/manufacturer-data-review-portal/DfT.ZEV.ManufacturerReview.Web/"]
COPY ["src/services/websites/manufacturer-data-review-portal/DfT.ZEV.ManufacturerReview.Domain/DfT.ZEV.ManufacturerReview.Domain.csproj", ".services/websites/manufacturer-data-review-portal/DfT.ZEV.ManufacturerReview.Domain/"]
COPY ["src/services/websites/manufacturer-data-review-portal/DfT.ZEV.ManufacturerReview.Application/DfT.ZEV.ManufacturerReview.Application.csproj", "./services/websites/manufacturer-data-review-portal/DfT.ZEV.ManufacturerReview.Application/"]
COPY ["src/services/websites/manufacturer-data-review-portal/DfT.ZEV.ManufacturerReview.Infrastructure/DfT.ZEV.ManufacturerReview.Infrastructure.csproj", "./services/websites/manufacturer-data-review-portal/DfT.ZEV.ManufacturerReview.Infrastructure/"]

COPY ["src/core/DfT.ZEV.Core.Application/DfT.ZEV.Core.Application.csproj", "./core/DfT.ZEV.Core.Application/"]
COPY ["src/core/DfT.ZEV.Core.Infrastructure/DfT.ZEV.Core.Infrastructure.csproj", "./core/DfT.ZEV.Core.Infrastructure/"]
COPY ["src/core/DfT.ZEV.Core.Domain/DfT.ZEV.Core.Domain.csproj", "./core/DfT.ZEV.Core.Domain/"]

COPY ["src/common/DfT.ZEV.Common/DfT.ZEV.Common.csproj", "./common/DfT.ZEV.Common/"]
COPY ["src/common/DfT.ZEV.Common.MVC.Authentication/DfT.ZEV.Common.MVC.Authentication.csproj", "./common/DfT.ZEV.Common.MVC.Authentication/"]

WORKDIR /src/services/websites/manufacturer-data-review-portal/DfT.ZEV.ManufacturerReview.Web
COPY src/Directory.Packages.props .
RUN dotnet restore

WORKDIR /src
COPY src/. .

WORKDIR /src/services/websites/manufacturer-data-review-portal/DfT.ZEV.ManufacturerReview.Web/frontend
RUN apk add --no-cache --repository=https://dl-cdn.alpinelinux.org/alpine/edge/main/ nodejs
RUN apk add --no-cache --repository=https://dl-cdn.alpinelinux.org/alpine/edge/community/ npm
RUN npm install -g pnpm
RUN pnpm install
RUN npm run build

FROM build AS publish
WORKDIR /src/services/websites/manufacturer-data-review-portal/DfT.ZEV.ManufacturerReview.Web
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
RUN apk add --no-cache tzdata
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./DfT.ZEV.ManufacturerReview.Web"]