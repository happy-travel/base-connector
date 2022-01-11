using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace HappyTravel.BaseConnector.Api.Infrastructure.Extensions;

public static class LoggerExtensions
{
    public static IDisposable AddScopedValue<T>(this ILogger<T> logger, string key, object value)
    {
        return logger.BeginScope(new Dictionary<string, object>
            {
                { key, value }
            });
    }
}
