using DfT.ZEV.Services.Organisations.Api;

var builder = WebApplication.CreateBuilder(args);
builder.SetupServices();

var app = builder.Build();
app.SetupWebApplication();

app.Run();