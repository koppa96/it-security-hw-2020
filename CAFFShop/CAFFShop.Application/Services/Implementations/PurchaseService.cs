using CAFFShop.Application.Services.Interfaces;
using CAFFShop.Dal;
using CAFFShop.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CAFFShop.Application.Services.Implementations
{
    public class PurchaseService : IPurchaseService
    {
        private readonly CaffShopContext context;
        private readonly IIdentityService identityService;

        public PurchaseService(CaffShopContext context, IIdentityService identityService)
        {
            this.context = context;
            this.identityService = identityService;
        }

        public async Task<Animation> GetAnimation(Guid animationId)
        {

            var userId = identityService.GetUserId();

            var animation = await context.Animations
                .Include(a => a.Preview)
                .FirstOrDefaultAsync(a => a.Id == animationId);

            if (await context.AnimationPurchases.AnyAsync(p => p.UserId == userId && p.Animation == animation) || animation.AuthorId == userId)
            {
                return null;
            }

            return animation;
        }
    }
}
