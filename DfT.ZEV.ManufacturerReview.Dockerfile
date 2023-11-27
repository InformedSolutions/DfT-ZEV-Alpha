#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
# Install latest LTS nodejs and npm
RUN apk add --no-cache --repository=https://dl-cdn.alpinelinux.org/alpine/edge/main/ nodejs
RUN apk add --no-cache --repository=https://dl-cdn.alpinelinux.org/alpine/edge/community/ npm


WORKDIR /src
COPY ["src/services/websites/manufacturer-data-review-portal/DfT.ZEV.ManufacturerReview.Web/DfT.ZEV.ManufacturerReview.Web.csproj", "DfT.ZEV.ManufacturerReview.Web/"]
COPY ["src/services/websites/manufacturer-data-review-portal/DfT.ZEV.ManufacturerReview.Domain/DfT.ZEV.ManufacturerReview.Domain.csproj", "DfT.ZEV.ManufacturerReview.Domain/"]
COPY ["src/services/websites/manufacturer-data-review-portal/DfT.ZEV.ManufacturerReview.Application/DfT.ZEV.ManufacturerReview.Application.csproj", "DfT.ZEV.ManufacturerReview.Application/"]
COPY ["src/services/websites/manufacturer-data-review-portal/DfT.ZEV.ManufacturerReview.Infrastructure/DfT.ZEV.ManufacturerReview.Infrastructure.csproj", "DfT.ZEV.ManufacturerReview.Infrastructure/"]
COPY ["src/common/DfT.ZEV.Common/DfT.ZEV.Common.csproj", "../../common/DfT.ZEV.Common/"]
COPY ["src/common/DfT.ZEV.Common.MVC.Authentication/DfT.ZEV.Common.MVC.Authentication.csproj", "../../common/DfT.ZEV.Common.MVC.Authentication/"]
RUN dotnet restore "DfT.ZEV.ManufacturerReview.Web/DfT.ZEV.ManufacturerReview.Web.csproj"  -r alpine-x64 /p:PublishReadyToRun=true

COPY ["src/services/websites/manufacturer-data-review-portal/", "."]
COPY ["src/common/", "../../../../common/"]
COPY stylecop.ruleset stylecop.ruleset

WORKDIR "/src/DfT.ZEV.ManufacturerReview.Web/frontend"
RUN npm install -g pnpm
RUN pnpm install
RUN npm run build

WORKDIR "/src/DfT.ZEV.ManufacturerReview.Web"
RUN dotnet build "DfT.ZEV.ManufacturerReview.Web.csproj" -c Release -o /app/build -r alpine-x64 /p:PublishReadyToRun=true

FROM build AS publish
RUN dotnet publish "DfT.ZEV.ManufacturerReview.Web.csproj" -c Release -o /app/publish --framework net6.0 -r alpine-x64 --self-contained true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishSingleFile=true

FROM base AS final
RUN apk add --no-cache tzdata
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./DfT.ZEV.ManufacturerReview.Web"]