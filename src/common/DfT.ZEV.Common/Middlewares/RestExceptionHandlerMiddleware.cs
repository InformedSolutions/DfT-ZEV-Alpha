using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Common.Middlewares;

public class RestExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger<RestExceptionHandlerMiddleware> _logger;

    public RestExceptionHandlerMiddleware(ILogger<RestExceptionHandlerMiddleware> logger) => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong, error: {ExceptionMessage}", ex.Message);
            throw;
        }
    }
}