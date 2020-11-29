using CAFFShop.Application.Services.Interfaces;
using CAFFShop.Dal;
using CAFFShop.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CAFFShop.Application.Services.Implementations
{
    public class PurchaseService : IPurchaseService
    {
        private readonly CaffShopContext context;
        private readonly IIdentityService identityService;
        private readonly ILogger logger;

        public PurchaseService(CaffShopContext context, IIdentityService identityService, ILogger<PurchaseService> logger)
        {
            this.context = context;
            this.identityService = identityService;
            this.logger = logger;
        }

        public async Task<Animation> GetAnimation(Guid animationId)
        {

            var userId = identityService.GetUserId();

            var animation = await getAnimation(animationId);

            if (animation == null)
            {
                logger.LogInformation("Animáció (Id: {0}) nem található", animationId);
                return null;
            }

            if (await context.AnimationPurchases.AnyAsync(p => p.UserId == userId && p.Animation == animation) || animation.AuthorId == userId)
            {
                logger.LogInformation("Felhasználó (Id: {0}) megvett vagy általa feltöltött animációt (Id: {1}) nem vehet meg", userId, animation.AuthorId);
                return null;
            }

            logger.LogInformation("Felhasználó (Id: {1}) animációt (Id: {0})  vásárolni szándékozik", userId, animationId);
            return animation;
        }

        public async Task<bool> Purchase(Guid animationId, string billingAddress, string billingName)
        {
            var userId = identityService.GetUserId();

            var animation = await getAnimation(animationId);
            if (animation == null)
            {
                logger.LogInformation("Animáció (Id: {0}) nem található", animationId);
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
            logger.LogInformation("Animáció (Id: {0}) felhasználó (Id: {1}) által megvásárolva", animationId, userId);
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
