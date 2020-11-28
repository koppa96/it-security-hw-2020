using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CAFFShop.Api.Infrastructure
{
    public class UserLoggerMiddleware
    {
        private readonly RequestDelegate next;

        public UserLoggerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var userId = context.User?.Claims?.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            IDisposable disposable = null;
            if (userId != null)
            {
                disposable = LogContext.PushProperty("UserId", userId);
            }

            await next(context);
            disposable?.Dispose();
        }
    }
}
