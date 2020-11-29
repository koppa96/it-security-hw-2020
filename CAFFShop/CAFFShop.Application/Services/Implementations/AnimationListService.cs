using CAFFShop.Application.Models;
using CAFFShop.Application.Services.Interfaces;
using CAFFShop.Dal;
using CAFFShop.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAFFShop.Application.Services.Implementations
{
    public class AnimationListService : IAnimationListService
    {
        private readonly CaffShopContext context;
        private readonly IIdentityService identityService;

        public AnimationListService(CaffShopContext context, IIdentityService identityService)
        {
            this.context = context;
            this.identityService = identityService;
        }
        public async Task<List<AnimationListModel>> ListAnimations()
        {
            var userId = identityService.GetUserId() ?? Guid.Empty;

            return await context.Animations
                .Include(a => a.Author)
                .Include(a => a.File)
                .Include(a => a.Preview)
                .Include(a => a.Comments)
                .Include(a => a.AnimationPurchases)
                .Include(a => a.Preview)
                .Where(a => a.ReviewState == ReviewState.Approved)
                .Select(a => new AnimationListModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    CreationDate = a.CreationTime.ToString("G"),
                    AuthorName = a.Author.UserName,
                    NumberOfComments = a.Comments.Count,
                    NumberOfPurchases = a.AnimationPurchases.Count,
                    Price = a.Price,
                    Own = a.AuthorId == userId,
                    HasPurchased = a.AnimationPurchases.Any(p => p.UserId == userId),
                    Preview = a.Preview.Path
                }).ToListAsync();

        }
    }
}
