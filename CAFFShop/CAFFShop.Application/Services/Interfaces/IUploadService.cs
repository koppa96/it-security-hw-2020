using CAFFShop.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAFFShop.Application.Services.Interfaces
{
	public interface IUploadService
	{
		Task<List<string>> AddAnimation(UploadDto dto);
	}
}
