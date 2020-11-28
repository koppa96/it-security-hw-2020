using CAFFShop.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace CAFFShop.Api.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Guid? GetUserId()
        {
            return Guid.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
