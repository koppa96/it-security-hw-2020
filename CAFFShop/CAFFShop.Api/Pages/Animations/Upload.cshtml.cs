using CAFFShop.Api.Models;
using CAFFShop.Application.Dtos;
using CAFFShop.Application.Extensions;
using CAFFShop.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace CAFFShop.Api.Pages.Animations
{
	[Authorize]
	public class UploadModel : PageModel
    {
		public UploadInputModel Model { get; set; }

		private IUploadService UploadService { get; }
		private IIdentityService IdentityService { get; }

		public UploadModel(IUploadService uploadService, IIdentityService identityService)
		{
			this.UploadService = uploadService;
			this.IdentityService = identityService;
		}

		public void OnGet()
        {

        }

		public async Task<IActionResult> OnPostAsync(UploadInputModel model)
		{
			if (!ModelState.IsValid)
				return Page();

			await UploadService.AddAnimation(new UploadDto()
			{
				Name = model.Name,
				Description = model.Description,
				Price = model.Price,
				File = await model.File.GetBytes(),
				UserId = IdentityService.GetUserId() ?? Guid.Empty
			});

			return Page();
		}
    }
}
