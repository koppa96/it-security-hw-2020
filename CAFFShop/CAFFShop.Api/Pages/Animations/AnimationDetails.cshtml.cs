using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAFFShop.Application.Services.Interfaces;
using CAFFShop.Dal;
using CAFFShop.Dal.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CAFFShop.Api.Pages.Animations
{
    public class AnimationDetailsModel : PageModel
    {
        private readonly CaffShopContext context;
        public readonly IIdentityService identityService;
        public readonly ICanDownloadService canDownloadService;

        public AnimationDetailsDTO AnimationDetails { get; set; }

        public AnimationDetailsModel(CaffShopContext context, IIdentityService identityService,ICanDownloadService canDownloadService)
        {
            this.context = context;
            this.identityService = identityService;
            this.canDownloadService = canDownloadService;
        }

        public async Task OnGetAsync(Guid? id)
        {
            var animation = await context.Animations
                .Include(x => x.Author)
                .Include(x => x.Comments)
                    .ThenInclude(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (animation == null)
            {
                throw new Exception("Animation not found");
            }

            AnimationDetails = await createAnimationDetailsDTO(animation);
        }

        public async Task<IActionResult> OnPostAsync(Guid id, string action)
        {
            var userId = identityService.GetUserId();

            if (userId == null)
            {
                throw new Exception("UserId is not found");
            }

            if (action == "commentSubmit")
            {
                var commentFromForm = Request.Form["comment"];
                var comment = new Comment
                {
                    AnimationId = id,
                    CreationTime = DateTime.Now,
                    Text = commentFromForm,
                    UserId = userId ?? throw new Exception("UserId is not found")
                };

                var animation = await context.Animations
                    .Include(x => x.Author)
                    .Include(x => x.Comments)
                        .ThenInclude(x => x.User)
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (animation == null)
                {
                    throw new Exception("Animation not found");
                }

                animation.Comments.Add(comment);
                
            } else if (action == "commentDelete")
            {

            }

            

            await context.SaveChangesAsync();
            return RedirectToPage();


        }

        private async Task<AnimationDetailsDTO> createAnimationDetailsDTO(Animation animation)
        {
            return new AnimationDetailsDTO
            {
                Id = animation.Id,
                Name = animation.Name,
                AuthorName = animation.Author?.UserName ?? "No user",
                Price = animation.Price,
                CreationTime = animation.CreationTime,
                Description = animation.Description,
                Comments = animation.Comments.Select(c => new CommentDTO
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.User.UserName,
                    CreationTime = c.CreationTime,
                    Text = c.Text
                }),
                CanDownloadCAFF = await canDownloadService.CanDownload(animation)
            };
        }

    }

    public class AnimationDetailsDTO 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime CreationTime { get; set; }
        public string AuthorName { get; set; }
        public IEnumerable<CommentDTO> Comments { get; set; }
        public bool CanDownloadCAFF { get; set; }

    }

    public class CommentDTO
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
        public DateTime CreationTime { get; set; }

        public Guid UserId { get; set; }
        public string UserName { get; set; }

    }
}
