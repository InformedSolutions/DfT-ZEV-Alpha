using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace DfT.ZEV.Services.ComplianceCalculation.Handler.Middleware;

public class GlobalErrorHandler : IMiddleware
{
    private readonly ILogger _logger;

    public GlobalErrorHandler(ILogger logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            // log the error
            _logger.Error(ex, "error during executing {Context} message: {Message}", context.Request.Path.Value,
                ex.Message);
            var response = context.Response;
            response.ContentType = "application/json";

            // get the response code and message
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await response.WriteAsync("Fatal failure.");
        }
    }
}