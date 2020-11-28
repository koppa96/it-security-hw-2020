using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CAFFShop.Api.Models
{
	public class UploadInputModel
	{
		[Display(Name = "Animáció neve: ", Prompt = "Animáció neve")]
		[Required(ErrorMessage = "Az animáció nevének megadása kötelező!")]
		public string Name { get; set; }

		[Display(Name = "Animáció leírása: ", Prompt = "Animáció leírása")]
		[DataType(DataType.MultilineText)]
		[Required(ErrorMessage = "Az animáció leírásának megadása kötelező!")]
		public string Description { get; set; }

		[Display(Name = "Kívánt ár: ", Prompt = "Ár")]
		[DataType(DataType.Currency)]
		[Required(ErrorMessage = "A kívánt ár megadása kötelező!")]
		public int Price { get; set; }

		[Display(Name = "Animációs fájl (.caff): ", Prompt = "Fájl")]
		[DataType(DataType.Upload)]
		[Required(ErrorMessage = "Az animációs fájl feltöltése kötelező!")]
		public IFormFile File { get; set; }
	}
}
