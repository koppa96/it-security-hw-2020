﻿using CAFFShop.Application.Services.Interfaces;
using CAFFShop.Dal.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
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
            var claim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            Guid id = Guid.Empty;

            if (claim != null)
            {
                id = Guid.Parse(claim.Value);
            }

            return id;
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
