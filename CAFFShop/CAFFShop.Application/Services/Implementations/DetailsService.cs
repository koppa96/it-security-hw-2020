﻿using CAFFShop.Application.Models;
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
    public class DetailsService : IDetailsService
    {
        private readonly CaffShopContext context;
        private readonly ICanDownloadService canDownloadService;
        private readonly IIdentityService identityService;
        private readonly ILogger<DetailsService> logger;

        public DetailsService(CaffShopContext context, ICanDownloadService canDownloadService, IIdentityService identityService, ILogger<DetailsService> logger)
        {
            this.context = context;
            this.canDownloadService = canDownloadService;
            this.identityService = identityService;
            this.logger = logger;
        }

        public async Task<AnimationDetailsModel> GetAnimationDetails(Guid id)
        {
            var animation = await context.Animations
                    .Include(x => x.Preview)
                    .Include(x => x.Author)
                    .Include(x => x.Comments)
                        .ThenInclude(x => x.User)
                    .SingleOrDefaultAsync(x => x.Id == id);

            if (animation == null)
            {
                logger.LogInformation("Kért animáció nem található");
                return null;
            }

            if (animation.ReviewState != ReviewState.Approved)
            {
                logger.LogInformation("Kért animáció nincs elfogadva");
                return null;
            }

            return await createAnimationDetailsDTO(animation);

        }

        public async Task DeleteComment(Guid commentId)
        {
            if (!identityService.IsAdmin())
            {
                logger.LogInformation("Komment törlés meghiusítva: felhasználó (Id: {0}) nem Admin", identityService.GetUserId());
                return;
            }

            var comment = await context.Comments.FindAsync(commentId);

            if (comment == null)
            {
                logger.LogInformation("Komment törlés meghiusítva: komment (Id: {0}) nem található", commentId);
                return;
            }

            context.Comments.Remove(comment);
            await context.SaveChangesAsync();

            logger.LogInformation("Komment (Id: {0}) törölve");
            return;

        }

        public async Task CreateComment(Guid animationId, string text)
        {
            var userId = identityService.GetUserId();

            var comment = new Comment
            {
                AnimationId = animationId,
                CreationTime = DateTime.Now,
                Text = text,
                UserId = userId.Value
            };

            var animation = await context.Animations
                .Include(x => x.Author)
                .Include(x => x.Comments)
                    .ThenInclude(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == animationId);

            if (animation == null)
            {
                logger.LogInformation("Animáció (Id: {0}) nem található");
                return;
            }

            animation.Comments.Add(comment);
            await context.SaveChangesAsync();
            logger.LogInformation("Sikeres kommentelés (AnimationId: {0}, UserId: {1})", animationId, userId);
        }


        private async Task<AnimationDetailsModel> createAnimationDetailsDTO(Animation animation)
        {
            return new AnimationDetailsModel
            {
                Id = animation.Id,
                Name = animation.Name,
                AuthorName = animation.Author?.UserName ?? "No user",
                Price = animation.Price,
                CreationTime = animation.CreationTime,
                Description = animation.Description,
                Comments = animation.Comments.Select(c => new CommentModel
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.User.UserName,
                    CreationTime = c.CreationTime,
                    Text = c.Text
                }),
                CanDownloadCAFF = await canDownloadService.CanDownload(animation),
                PreviewFile = animation.Preview?.Path
            };
        }


    }
}
