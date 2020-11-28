using CAFFShop.Application.Services.Interfaces;
using CAFFShop.Dal;
using CAFFShop.Dal.Constants;
using CAFFShop.Dal.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CAFFShop.Api.Services
{
    public class CanDownloadService : ICanDownloadService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly CaffShopContext context;

        public CanDownloadService(IHttpContextAccessor httpContextAccessor, CaffShopContext context)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;
        }
        public async Task<bool> CanDownload(Animation animation)
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (userId == null)
            {
                return false;
            }

            var user = await context.Users
                .Include(x => x.AnimationPurchases)
                .Include(x => x.UploadedAnimations)
                .SingleOrDefaultAsync(x => x.Id == Guid.Parse(userId));

            return IsAuthenticated() && (IsPurchased(user, animation) || IsAuthor(user, animation) || IsAdmin());

        }
        private bool IsAuthenticated()
        {
            return httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        private bool IsAdmin()
        {
            return httpContextAccessor.HttpContext.User.Claims.Any(x => 
                x.Type == ClaimTypes.Role && x.Value == RoleTypes.Admin
            );
        }

        private bool IsPurchased(User user, Animation animation)
        {
            return user.AnimationPurchases.SingleOrDefault(x => x.AnimationId == animation.Id) != null;
        }

        private bool IsAuthor(User user, Animation animation)
        {
            return animation.AuthorId == user.Id;
        }
    }
}
