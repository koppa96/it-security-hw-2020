using CAFFShop.Api.Models;
using CAFFShop.Application.Dtos;
using CAFFShop.Application.Extensions;
using CAFFShop.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace CAFFShop.Api.Pages.Animations
{
	public class UploadModel : PageModel
    {
		public UploadInputModel Model { get; set; }

		private IUploadService UploadService { get; }

		public UploadModel(IUploadService uploadService)
		{
			this.UploadService = uploadService;
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
				//UserId = 
			});

			return Page();
		}
    }
}
