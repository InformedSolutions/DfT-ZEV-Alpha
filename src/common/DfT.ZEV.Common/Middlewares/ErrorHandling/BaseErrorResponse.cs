#nullable enable
using Microsoft.AspNetCore.Http;

namespace DfT.ZEV.Common.Middlewares.ErrorHandling;

public class BaseData
{
    public string Message { get; set; } = default!;
}

public class BaseErrorResponse
{
    public BaseData? Data { get; set; } = default;
    public int StatusCode { get; set; } = StatusCodes.Status500InternalServerError;
}