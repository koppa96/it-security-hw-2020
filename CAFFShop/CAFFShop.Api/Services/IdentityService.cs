using CAFFShop.Application.Services.Interfaces;
using CAFFShop.Dal.Constants;
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

        public bool IsAdmin()
        {
            return httpContextAccessor.HttpContext.User.Claims.Any(x =>
                x.Type == ClaimTypes.Role && x.Value == RoleTypes.Admin
            );
        }

        public bool IsAuthenticated()
        {
            return httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

    }
}
