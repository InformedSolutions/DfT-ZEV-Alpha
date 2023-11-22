using Microsoft.EntityFrameworkCore;
using Zev.Core.Infrastructure.Configuration;
using Zev.Core.Infrastructure.Logging;
using Zev.Core.Infrastructure.Persistence;
using Zev.Core.Infrastructure.Repositories;
using Zev.Services.ProcessMonitoringService.Host.Features;

namespace Zev.Services.ProcessMonitoringService.Host;

public static class Setup
{
   public static WebApplicationBuilder SetupServices(this WebApplicationBuilder builder)
   {
      var postgresSettings = builder.Services.ConfigurePostgresSettings(builder.Configuration);
      
      builder.Services.AddDbContext<AppDbContext>(opt =>
      {
         opt.UseNpgsql(postgresSettings.ConnectionString, conf =>
         {
            conf.EnableRetryOnFailure(5, TimeSpan.FromSeconds(20), new List<string> { "4060" });
         });
      });
      builder.Services.AddSerilog(builder.Configuration);
      builder.Services.AddRepositories();
      return builder;
   }

   public static WebApplication SetupWebApplication(this WebApplication app)
   {
      
      app.MapGet("/", () => "Hello World!");
      app.MapEndpoints();
      return app;
   }
}