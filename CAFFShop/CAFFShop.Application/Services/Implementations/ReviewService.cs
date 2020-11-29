using CAFFShop.Application.Models;
using CAFFShop.Application.Services.Interfaces;
using CAFFShop.Dal;
using CAFFShop.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAFFShop.Application.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly CaffShopContext context;
        private readonly IIdentityService identityService;

        public ReviewService(CaffShopContext context, IIdentityService identityService)
        {
            this.context = context;
            this.identityService = identityService;
        }

        public async Task<bool> Review(Guid animationId, ReviewState reviewState)
        {

            var anim = await context.Animations.FindAsync(animationId);

            if (anim == null)
            {
                return false;
            }

            anim.ReviewedById = identityService.GetUserId();
            anim.ReviewState = reviewState;
            await context.SaveChangesAsync();
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
