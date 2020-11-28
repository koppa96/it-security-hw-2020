using CAFFShop.Application.Dtos;
using System.Threading.Tasks;

namespace CAFFShop.Application.Services.Interfaces
{
	public interface IUploadService
	{
		Task AddAnimation(UploadDto dto);
	}
}
