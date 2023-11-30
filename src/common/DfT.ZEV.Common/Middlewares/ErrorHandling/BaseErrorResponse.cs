#nullable enable
using Microsoft.AspNetCore.Http;

namespace DfT.ZEV.Common.Middlewares.ErrorHandling;

public class BaseErrorResponse
{
    public object? Data { get; set; } = default;
    public int StatusCode { get; set; } = StatusCodes.Status500InternalServerError;
}