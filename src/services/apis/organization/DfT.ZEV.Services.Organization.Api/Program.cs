using DfT.ZEV.Services.Organization.Api;

var builder = WebApplication.CreateBuilder(args);
builder.SetupServices();

var app = builder.Build();
app.SetupWebApplication();

app.Run();