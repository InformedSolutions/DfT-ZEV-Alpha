using Zev.Services.ProcessMonitoringService.Host;

var builder = WebApplication.CreateBuilder(args);
builder.SetupServices();

var app = builder.Build();
app.SetupWebApplication();

app.Run();