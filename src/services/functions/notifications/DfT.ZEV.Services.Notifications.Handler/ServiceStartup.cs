using System;
using System.Collections.Generic;
using System.IO;
using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Core.Application;
using DfT.ZEV.Core.Infrastructure;
using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DfT.ZEV.Core.Infrastructure.Persistence;

namespace DfT.ZEV.Services.Notifications.Handler;

public class ServiceStartup : FunctionsStartup
{
    public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        var configuration = BuildConfiguration();
        ConfigureServices(services, configuration);
    }

    private static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositories();
        services.AddApplication();
        services.AddApplication();
        services.AddLogging();
    }
}