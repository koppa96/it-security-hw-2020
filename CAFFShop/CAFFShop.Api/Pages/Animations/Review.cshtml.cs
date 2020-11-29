using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CAFFShop.Dal;
using CAFFShop.Dal.Entities;
using System.Security.Claims;
using CAFFShop.Dal.Constants;
using Microsoft.AspNetCore.Authorization;
using CAFFShop.Api.Infrastructure.Filters;

namespace CAFFShop.Api.Pages.Animations
{
    [Authorize(Roles = RoleTypes.Admin)]
    [LogRequestsFilter]
    public class ReviewModel : PageModel
    {
        private readonly CAFFShop.Dal.CaffShopContext _context;
        private readonly CAFFShop.Application.Services.Interfaces.IIdentityService identityService;

        public ReviewModel(CAFFShop.Dal.CaffShopContext context, CAFFShop.Application.Services.Interfaces.IIdentityService identityService)
        {
            _context = context;
            this.identityService = identityService;
        }

        public IEnumerable<AnimationReviewDto> Animations { get; set; }

        public IActionResult OnGet()
        {
            if (!HttpContext.User.Claims.Any(x => x.Type == ClaimTypes.Role && x.Value == RoleTypes.Admin))
            {
                return RedirectToPage("/Animations/Index");
            }

            LoadAnimations();
            return Page();
        }

        private void LoadAnimations()
        {
            Animations = _context
                .Animations
                .Include(a => a.Author)
                .Include(a => a.Preview)
                .Where(a => a.ReviewState == ReviewState.Pending)
                .Select(a => new AnimationReviewDto
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

        public async Task<IActionResult> OnPostReject(string id)
        {
            if(!HttpContext.User.Claims.Any(x => x.Type == ClaimTypes.Role && x.Value == RoleTypes.Admin))
            {
                return RedirectToPage("/Animations/Index");
            }

            if (Guid.TryParse(id, out Guid animId))
            {
                var anim = await _context.Animations.FindAsync(animId);
                anim.ReviewedById = identityService.GetUserId();
                anim.ReviewState = ReviewState.Rejected;
                await _context.SaveChangesAsync();
            }
            LoadAnimations();
            return Page();
        }

        public async Task<IActionResult> OnPostApprove(string id)
        {
            if (!HttpContext.User.Claims.Any(x => x.Type == ClaimTypes.Role && x.Value == RoleTypes.Admin))
            {
                return RedirectToPage("/Animations/Index");
            }

            if (Guid.TryParse(id, out Guid animId))
            {
                var anim = await _context.Animations.FindAsync(animId);
                anim.ReviewedById = identityService.GetUserId();
                anim.ReviewState = ReviewState.Approved;
                await _context.SaveChangesAsync();
            }
            LoadAnimations();
            return Page();
        }
    }
    public class AnimationReviewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Preview { get; set; }
        public string Description { get; set; }
        public string CreationDate { get; set; }
        public string AuthorName { get; set; }
        public int Price { get; set; }
    }
}
