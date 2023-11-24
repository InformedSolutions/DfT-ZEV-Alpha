using System.ComponentModel.DataAnnotations;
using System.Net;
using DfT.ZEV.Common.Exceptions;
using DfT.ZEV.Common.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Common.Middlewares;

/// <summary>
/// This middleware will catch unhandled errors
/// and return the exception as a JSON result object.
/// </summary>
public class JsonApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerFactory _loggerFactory;

    /// <summary>
    /// Constructor used by DI.
    /// </summary>
    /// <param name="next"><see cref="RequestDelegate"/>.</param>
    /// <param name="loggerFactory"><see cref="ILoggerFactory"/>.</param>
    public JsonApiExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _loggerFactory = loggerFactory;
    }

    /// <summary>
    /// Public entrypoint for async invocation as part of request processing chain.
    /// </summary>
    /// <param name="httpContext"><see cref="HttpContext"/>.</param>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        var logger = _loggerFactory.CreateLogger<JsonApiExceptionMiddleware>();

        try
        {
            await _next(httpContext);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation exception raised. Details: {ExceptionMessage}", ex.Message);
            await HandleWithHttpStatusBaseExceptionAsync(httpContext, new WithHttpStatusBaseException(ex.Message, HttpStatusCode.BadRequest));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Something went wrong, error: {ExceptionMessage}", ex.Message);
            await HandleExceptionAsync(httpContext);
        }
    }

    /// <summary>
    /// Handles base exceptions and inserts an error code into the http response.
    /// </summary>
    /// <param name="context"><see cref="HttpContext"/>.</param>
    /// <param name="ex"><see cref="WithHttpStatusBaseException"/>.</param>
    private Task HandleWithHttpStatusBaseExceptionAsync(HttpContext context, WithHttpStatusBaseException ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)ex.StatusCode;

        return context.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = ex.Message,
        }.ToString());
    }

    /// <summary>
    /// Handles base exceptions.
    /// </summary>
    /// <param name="context"><see cref="HttpContext"/>.</param>
    private Task HandleExceptionAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return context.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = "Internal Server Error.",
        }.ToString());
    }
}