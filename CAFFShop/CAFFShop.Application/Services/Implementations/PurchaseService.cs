using CAFFShop.Application.Services.Interfaces;
using CAFFShop.Dal;
using CAFFShop.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
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

            var animation = await getAnimation(animationId);

            if (animation == null)
            {
                return null;
            }

            if (await context.AnimationPurchases.AnyAsync(p => p.UserId == userId && p.Animation == animation) || animation.AuthorId == userId)
            {
                return null;
            }

            return animation;
        }

        public async Task<bool> Purchase(Guid animationId, string billingAddress, string billingName)
        {
            var userId = identityService.GetUserId();

            var animation = await getAnimation(animationId);
            if (animation == null)
            {
                return false;
            }

            var animationPurchase = new AnimationPurchase()
            {
                Id = Guid.NewGuid(),
                Animation = animation,
                UserId = userId.Value,
                BillingAddress = billingAddress,
                BillingName = billingName,
                PriceAtPurchase = animation.Price,
                CreationTime = DateTime.Now
            };
            context.AnimationPurchases.Add(animationPurchase);
            await context.SaveChangesAsync();
            return true;

        }

        private async Task<Animation> getAnimation(Guid animationId)
        {
            return await context.Animations
                .Include(a => a.Preview)
                .FirstOrDefaultAsync(a => a.Id == animationId);
        }
    }
}
