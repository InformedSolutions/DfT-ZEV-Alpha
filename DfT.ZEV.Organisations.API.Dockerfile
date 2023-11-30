#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build


WORKDIR /src
COPY ["src/services/apis/organisation/DfT.ZEV.Services.Organisation.Api/DfT.ZEV.Services.Organisation.Api.csproj", "DfT.ZEV.Services.Organisation.Api/"]
COPY ["src/core/DfT.ZEV.Core.Application/DfT.ZEV.Core.Application.csproj", "../../../../core/DfT.ZEV.Core.Application/"]
COPY ["src/core/DfT.ZEV.Core.Infrastructure/DfT.ZEV.Core.Infrastructure.csproj", "../../../../core/DfT.ZEV.Core.Infrastructure/"]
COPY ["src/core/DfT.ZEV.Core.Domain/DfT.ZEV.Core.Domain.csproj", "../../../../core/DfT.ZEV.Core.Domain/"]
COPY ["src/common/DfT.ZEV.Common/DfT.ZEV.Common.csproj", "../../../../common/DfT.ZEV.Common/"]
COPY ["src/common/DfT.ZEV.Common.MVC.Authentication/DfT.ZEV.Common.MVC.Authentication.csproj", "../../../../common/DfT.ZEV.Common.MVC.Authentication/"]
RUN dotnet restore "DfT.ZEV.Services.Organisation.Api/DfT.ZEV.Services.Organisation.Api.csproj"  -r alpine-x64 /p:PublishReadyToRun=true

COPY ["src/services/apis/organisation/", "."]
COPY ["src/core/", "../../core/"]
COPY ["src/common/", "../../common/"]
COPY stylecop.ruleset stylecop.ruleset

WORKDIR "/src/DfT.ZEV.Services.Organisation.Api"
RUN dotnet build "DfT.ZEV.Services.Organisation.Api.csproj" -c Release -o /app/build -r alpine-x64 /p:PublishReadyToRun=true

FROM build AS publish
RUN dotnet publish "DfT.ZEV.Services.Organisation.Api.csproj" -c Release -o /app/publish --framework net6.0 -r alpine-x64 --self-contained true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishSingleFile=true

FROM base AS final
RUN apk add --no-cache tzdata
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./DfT.ZEV.Services.Organisation.Api"]