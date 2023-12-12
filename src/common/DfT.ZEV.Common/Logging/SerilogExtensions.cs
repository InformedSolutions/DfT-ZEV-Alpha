using DfT.ZEV.Common.Logging.Enrichers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;


namespace DfT.ZEV.Common.Logging;

public static class SerilogExtensions
{
  public static void UseSerilog(this WebApplicationBuilder builder)
  {
    builder.Services.ForwardHeaders();
    builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    {
      loggerConfiguration
              .ReadFrom.Configuration(hostingContext.Configuration)
              .Enrich.WithEnvironmentName()
              .Enrich.WithCorrelationId()
              .Enrich.WithBuildId();
    });
  }

  public static IHostBuilder UseSerilog(this IHostBuilder builder)
  {
    return builder.UseSerilog((hostingContext, loggerConfiguration) =>
    {
      loggerConfiguration
              .ReadFrom.Configuration(hostingContext.Configuration)
              .Enrich.WithEnvironmentName()
              .Enrich.WithCorrelationId()
              .Enrich.WithBuildId();
    });
  }

  public static void ForwardHeaders(this IServiceCollection services)
  {
    services.Configure<ForwardedHeadersOptions>(options =>
       {
         options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
       });
  }
}
