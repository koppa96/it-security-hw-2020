using CAFFShop.Application.Models;
using CAFFShop.Application.Services.Interfaces;
using CAFFShop.Dal;
using CAFFShop.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAFFShop.Application.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly CaffShopContext context;
        private readonly IIdentityService identityService;
        private readonly ILogger<ReviewService> logger;

        public ReviewService(CaffShopContext context, IIdentityService identityService, ILogger<ReviewService> logger)
        {
            this.context = context;
            this.identityService = identityService;
            this.logger = logger;
        }

        public async Task<bool> Review(Guid animationId, ReviewState reviewState)
        {

            var anim = await context.Animations.FindAsync(animationId);

            if (anim == null)
            {
                logger.LogInformation("Animáció (Id: {0}) nem található", animationId);
                return false;
            }

            anim.ReviewedById = identityService.GetUserId();
            anim.ReviewState = reviewState;
            await context.SaveChangesAsync();
            logger.LogInformation("Animáció (Id: {0}) felülvizsgálva felhasználó (Id: {1}) által következő állapottal: {2}", animationId, identityService.GetUserId(), reviewState.ToString("G"));
            return true;
        }


        public IEnumerable<AnimationReviewModel> LoadAnimations()
        {
            return context
                .Animations
                .Include(a => a.Author)
                .Include(a => a.Preview)
                .Where(a => a.ReviewState == ReviewState.Pending)
                .Select(a => new AnimationReviewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    CreationDate = a.CreationTime.ToString("G"),
                    AuthorName = a.Author.UserName,
                    Price = a.Price,
                    Preview = a.Preview.Path
                });
        }

    }
}
