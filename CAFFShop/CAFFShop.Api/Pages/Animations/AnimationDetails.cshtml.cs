using CAFFShop.Application.Services.Interfaces;
using CAFFShop.Dal;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAFFShop.Api.Pages.Animations
{
    public class AnimationDetailsModel : PageModel
    {
        private readonly CaffShopContext context;
        public readonly ICanDownloadService canDownloadService;

        public AnimationDetailsDTO AnimationDetails { get; set; }

        public AnimationDetailsModel(CaffShopContext context, ICanDownloadService canDownloadService)
        {
            this.context = context;
            this.canDownloadService = canDownloadService;
        }

        public async Task OnGetAsync(Guid? id)
        {
            var animation = await context.Animations
                .Include(x => x.Author)
                .Include(x => x.Preview)
                .Include(x => x.Comments)
                    .ThenInclude(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (animation == null)
            {
                throw new Exception("Animation not found");
            }

            AnimationDetails = new AnimationDetailsDTO
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
                CanDownloadCAFF = await canDownloadService.CanDownload(animation),
                PreviewFile = animation.Preview.Path
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
		public string PreviewFile { get; set; }

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
