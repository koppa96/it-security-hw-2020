using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CAFFShop.Dal;
using CAFFShop.Dal.Entities;

namespace CAFFShop.Api.Pages.Animations
{
    public class IndexModel : PageModel
    {
        private readonly CaffShopContext _context;

        public IndexModel(CaffShopContext context)
        {
            _context = context;
        }

        public IList<AnimationDto> Animation { get;set; }

        public async Task OnGetAsync()
        {
            var userId = Guid.Empty;
            var claim = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (claim != null) {
                userId = Guid.Parse(claim.Value);
            }

            Animation = await _context.Animations
                .Include(a => a.Author)
                .Include(a => a.File)
                .Include(a => a.Preview)
                .Include(a => a.Comments)
                .Include(a => a.AnimationPurchases)
                .Select(a => new AnimationDto
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
                    HasPurchased = a.AnimationPurchases.Any(p => p.UserId == userId)
                }).ToListAsync();
        }
    }

    public class AnimationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreationDate { get; set; }
        public string AuthorName { get; set; }
        public int NumberOfPurchases { get; set; }
        public int NumberOfComments { get; set; }
        public int Price { get; set; }
        public bool Own { get; set; }
        public bool HasPurchased { get; set; }
        
    }
}
