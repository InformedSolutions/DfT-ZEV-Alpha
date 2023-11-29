using System.Net;
using System.Text.Json;
using DfT.ZEV.Core.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Common.Middlewares.ErrorHandling;

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

            var response = GetResponse(ex);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

    private static BaseErrorResponse GetResponse(Exception ex)
        => ex switch
        {
            HandlerException handlerException => HandleHandlerException(handlerException),
            _ => HandleException(ex)
        };
    
    private static BaseErrorResponse HandleHandlerException(HandlerException exception)
    {
        var response = new BaseErrorResponse
        {
            StatusCode = (int)HttpStatusCode.BadRequest,
            Data = new { exception.Message}
        };

        return response;
    }
    private static BaseErrorResponse HandleException(Exception exception)
    {
        var response = new BaseErrorResponse
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Data = new { exception.Message }
        };

        return response;
    }
}