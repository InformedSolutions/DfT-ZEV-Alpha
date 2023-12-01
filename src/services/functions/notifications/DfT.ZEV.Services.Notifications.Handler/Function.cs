using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DfT.ZEV.Common.Configuration;
using Google.Cloud.Functions.Framework;
using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog.Context;
using DfT.ZEV.Core.Domain.Processes.Services;
using DfT.ZEV.Core.Domain.Processes.Values;
using DfT.ZEV.Core.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace DfT.ZEV.Services.Notifications.Handler;

/// <summary>
///     Represents an HTTP function that sends notifications (e.g. email, SMS).
/// </summary>
[FunctionsStartup(typeof(ServiceStartup))]
public class Function : IHttpFunction
{
    private readonly ILogger<Function> _logger;

    public Function(ILogger<Function> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     Handles an request to send a notification.
    /// </summary>
    public async Task HandleAsync(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private async Task Run(HttpContext context)
    {
        throw new NotImplementedException();
    }
}