using System.Collections.Generic;
using System.Web;
using DfT.ZEV.Administration.Web.Extensions.Conventions;
using DfT.ZEV.Administration.Web.Extensions.TagHelpers;
using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Common.Logging;
using DfT.ZEV.Common.Middlewares;
using DfT.ZEV.Common.MVC.Authentication.HealthChecks;
using DfT.ZEV.Common.MVC.Authentication.HealthChecks.CustomHealthChecks;
using DfT.ZEV.Common.MVC.Authentication.Identity;
using DfT.ZEV.Common.MVC.Authentication.ServiceCollectionExtensions;
using DfT.ZEV.Common.Notifications;
using DfT.ZEV.Common.Security;
using DfT.ZEV.Core.Application;
using DfT.ZEV.Core.Application.Clients;
using DfT.ZEV.Core.Infrastructure;
using DfT.ZEV.Core.Infrastructure.Persistence;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Serilog;


namespace DfT.ZEV.Administration.Web;

public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Configuration = configuration;
        Environment = environment;
    }

    public IWebHostEnvironment Environment { get; }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        if (Configuration.GetSection("BasicAuth").GetValue<bool>("IsEnabled"))
        {
            services.AddBasicAuthentication();
        }

        services.AddAntiforgery(opts => opts.Cookie.Name = "Antiforgery");
        services.AddSession(options => { options.Cookie.Name = "Session"; });
    
        services.AddControllersWithViews()
            .AddMvcOptions(options =>
            {
                options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
                    (val, prop) => $"The value '{HttpUtility.HtmlEncode(val)}' is not valid for {prop}.");
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                    (val) => $"The value '{HttpUtility.HtmlEncode(val)}' is invalid.");
                options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(
                    (val) => $"The value '{HttpUtility.HtmlEncode(val)}' is invalid.");
                options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(
                    (val) => $"The value '{HttpUtility.HtmlEncode(val)}' is not valid.");

                options.Conventions.Add(new RemoveActionConvention(Configuration));

                options.Conventions.Add(new BasicAuthConvention(Configuration));
            });

        var secureCookiePolicy = Configuration.GetValue<bool>("SslHosted")
            ? CookieSecurePolicy.Always
            : CookieSecurePolicy.None;
        
        services.AddTransient<INotificationService, NotificationService>();
        
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.Secure = secureCookiePolicy;
            options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
            options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
            options.ConsentCookie.Name = "AdditionalCookiesConsent";
        });

        services.Configure<CookieTempDataProviderOptions>(options =>
        {
            options.Cookie.IsEssential = true;
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
            options.Cookie.SecurePolicy = secureCookiePolicy;
        });

        services.AddHttpContextAccessor();

        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration)
            .CreateLogger();

        var postgresSettings = services.ConfigurePostgresSettings(this.Configuration);
        services.ConfigureGoogleCloudSettings(this.Configuration);
        services.AddIdentityPlatform(Configuration);
        services.AddDbContextPool<AppDbContext>(opt =>
        {
            // This causes errors while working in multi-threaded processing, need to deep dive this topic
            //  opt.UseNpgsql(configuration.ConnectionString,
            //     conf => { conf.EnableRetryOnFailure(5, TimeSpan.FromSeconds(20), new List<string> { "4060" }); });
            opt.UseNpgsql(postgresSettings.ConnectionString);
        });
        services.AddApplication();
        services.AddInfrastructureServices();

        services.AddApiServiceClients(Configuration);
        services.ForwardHeaders();

        services.AddOptions();
        services.ConfigureServicesSettings(Configuration);
        services.AddGovUkFrontend(options => options.AddImportsToHtml = false);

        services.AddResponseCompression();

        //services.AddHealthChecks();
        services.AddHealthChecks()
                 .AddCheck<RestServiceHealthCheck>("organization-api-service", HealthStatus.Unhealthy);
        // Register the Google Analytics configuration
        services.Configure<GoogleAnalyticsOptions>(options =>
            Configuration.GetSection("GoogleAnalytics").Bind(options));

        // Register the TagHelperComponent
        services.AddTransient<ITagHelperComponent, GoogleAnalyticsTagHelperComponent>();

        services.AddAuthServices();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionHandler("/Error/500");
        app.UseMiddleware<WebsiteExceptionMiddleware>();
        app.UseMiddleware<CorrelationIdLoggerMiddleware>();
        app.UseIdentity();
        var allowedHostnames = Configuration.GetValue<string>("AllowedHostnames").Split(",");

        app.UseAllowedHostFilteringMiddleware(new HostFilteringOptions
        {
            AllowedHostnames = new List<string>(allowedHostnames),
            ExcludedPaths = new List<string> { "/health" },
            ErrorRoute = "/Error/400",
        });

        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();

        if (!env.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        var cachingPragmaHeaderInHours = Configuration.GetValue<int>("StaticFilesCachePragmaInHours");
        var cachePragmaInSeconds = 60 * 60 * cachingPragmaHeaderInHours;

        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = ctx =>
            {
                SecurityHeadersHelper.ConfigureCommonSecurityHeaders(
                    ctx.Context, Configuration.GetValue<string>("AdditionalCspSourceValues"));

                // Allow static file caching by overriding default header value
                ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                    "public,max-age=" + cachePragmaInSeconds;
                ctx.Context.Response.Headers.Remove(HeaderNames.Pragma);
            },
        });

        app.UseCookiePolicy();

        app.UseResponseCompression();

        app.Use(async (ctx, next) =>
        {
            SecurityHeadersHelper.ConfigureCommonSecurityHeaders(
                ctx, Configuration.GetValue<string>("AdditionalCspSourceValues"));

            await next();
            if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
            {
                // Re-execute the request so the user gets the error page
                ctx.Request.Path = "/Error/404";
                await next();
            }
        });

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSession();
        app.UseMiddleware<PageViewLoggerMiddleware>();


        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapAreaControllerRoute(
                name: "Authentication",
                areaName: "Authentication",
                pattern: "{controller=Home}/{action=Index}");
            endpoints.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}");
        });

        app.UseHealthChecksMvc();
    }
}
