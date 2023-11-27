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
COPY ["src/services/websites/zev-administration-portal/DfT.ZEV.Administration.Web/DfT.ZEV.Administration.Web.csproj", "DfT.ZEV.Administration.Web/"]
COPY ["src/services/websites/zev-administration-portal/DfT.ZEV.Administration.Domain/DfT.ZEV.Administration.Domain.csproj", "DfT.ZEV.Administration.Domain/"]
COPY ["src/services/websites/zev-administration-portal/DfT.ZEV.Administration.Application/DfT.ZEV.Administration.Application.csproj", "DfT.ZEV.Administration.Application/"]
COPY ["src/services/websites/zev-administration-portal/DfT.ZEV.Administration.Infrastructure/DfT.ZEV.Administration.Infrastructure.csproj", "DfT.ZEV.Administration.Infrastructure/"]
COPY ["src/common/DfT.ZEV.Common/DfT.ZEV.Common.csproj", "../../common/DfT.ZEV.Common/"]
COPY ["src/common/DfT.ZEV.Common.MVC.Authentication/DfT.ZEV.Common.MVC.Authentication.csproj", "../../common/DfT.ZEV.Common.MVC.Authentication/"]
RUN dotnet restore "DfT.ZEV.Administration.Web/DfT.ZEV.Administration.Web.csproj"  -r alpine-x64 /p:PublishReadyToRun=true

COPY ["src/services/websites/zev-administration-portal/", "."]
COPY ["src/common/", "../../../../common/"]
COPY stylecop.ruleset stylecop.ruleset

WORKDIR "/src/DfT.ZEV.Administration.Web/frontend"
RUN npm install -g pnpm
RUN pnpm install
RUN npm run build

WORKDIR "/src/DfT.ZEV.Administration.Web"
RUN dotnet build "DfT.ZEV.Administration.Web.csproj" -c Release -o /app/build -r alpine-x64 /p:PublishReadyToRun=true

FROM build AS publish
RUN dotnet publish "DfT.ZEV.Administration.Web.csproj" -c Release -o /app/publish --framework net6.0 -r alpine-x64 --self-contained true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishSingleFile=true

FROM base AS final
RUN apk add --no-cache tzdata
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./DfT.ZEV.Administration.Web"]