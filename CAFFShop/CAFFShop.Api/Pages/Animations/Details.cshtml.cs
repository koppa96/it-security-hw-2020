using CAFFShop.Application.Models;
using CAFFShop.Application.Services.Interfaces;
using CAFFShop.Dal;
using CAFFShop.Dal.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CAFFShop.Api.Pages.Animations
{
	public class DetailsModel : PageModel
    {
        private readonly IDetailsService detailsService;

        public IDownloadService DownloadService { get; set; }

        public AnimationDetailsModel AnimationDetails { get; set; }

        public DetailsModel(IDownloadService downloadService, IDetailsService detailsService)
        {
            this.DownloadService = downloadService;
            this.detailsService = detailsService;
        }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            AnimationDetails = await detailsService.GetAnimationDetails(id.Value);

            if (AnimationDetails == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteCommentAsync(Guid commentId)
        {
            await detailsService.DeleteComment(commentId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAsync(Guid id, string action)
        {
            if (action == "commentSubmit")
            {
                await detailsService.CreateComment(id, Request.Form["comment"].ToString());
            }
            
            return RedirectToPage();    
        }

        public async Task<IActionResult> OnPostDownloadAnimation(Guid id)
        {
            Stream stream = await DownloadService.GetFile(id);

            if (stream == null || stream.Length == 0)
            {
                ModelState.AddModelError("", "Sikertelen letöltés!");
                await OnGetAsync(id);
                return Page();
            }

            return File(stream, "application/octet-stream", $"{AnimationDetails.Name}.caff");
        }
    }


}
