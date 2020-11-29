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
using CAFFShop.Application.Models;
using CAFFShop.Application.Services.Interfaces;

namespace CAFFShop.Api.Pages.Animations
{
    [Authorize(Roles = RoleTypes.Admin)]
    [LogRequestsFilter]
    public class ReviewModel : PageModel
    {

        private readonly IReviewService reviewService;
        private readonly IDownloadService downloadService;

        public ReviewModel(IReviewService reviewService, IDownloadService downloadService)
        {
            this.reviewService = reviewService;
            this.downloadService = downloadService;
        }

        public IEnumerable<AnimationReviewModel> Animations { get; set; }

        public IActionResult OnGet()
        {
            Animations = reviewService.LoadAnimations();
            return Page();
        }

        public async Task<IActionResult> OnPostReject(string id)
        {
            
            if (Guid.TryParse(id, out Guid animId))
            {
                var success = await reviewService.Review(animId, ReviewState.Rejected);
                if (!success)
                {
                    return RedirectToPage();
                }
            }
            Animations = reviewService.LoadAnimations();
            return Page();
        }

        public async Task<IActionResult> OnPostApprove(string id)
        {

            if (Guid.TryParse(id, out Guid animId))
            {
                var success = await reviewService.Review(animId, ReviewState.Approved);
                if (!success)
                {
                    return RedirectToPage();
                }
            }
            Animations = reviewService.LoadAnimations();
            return Page();
        }

        public async Task<IActionResult> OnPostDownload(Guid id)
        {
            var stream = await downloadService.GetFile(id);
            var details = reviewService.LoadAnimations().Single(x => x.Id == id);

            if (stream == null || stream.Length == 0)
            {
                ModelState.AddModelError("", "Sikertelen letöltés!");
                return OnGet();
            }

            return File(stream, "application/octet-stream", $"{details.Name}.caff");
        }
    }
}
