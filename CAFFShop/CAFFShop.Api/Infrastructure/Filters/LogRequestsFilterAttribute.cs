using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Diagnostics;

namespace CAFFShop.Api.Infrastructure.Filters
{
    public class LogRequestsFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<LogRequestsFilterAttribute>>();
            var requestPath = LogContext.PushProperty("RequestPath", context.HttpContext.Request.Path.ToString());
            var method = LogContext.PushProperty("Method", context.HttpContext.Request.Method);

            var statusCode = LogContext.PushProperty("Status", context.HttpContext.Response.StatusCode);
            logger.LogInformation($"Finished executing request {context.HttpContext.Request.Method} {context.HttpContext.Request.Path}. Status Code: {context.HttpContext.Response.StatusCode}");

            requestPath.Dispose();
            method.Dispose();
            statusCode.Dispose();
            base.OnResultExecuted(context);
        }
    }
}
