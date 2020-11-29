using System;
using System.IO;
using System.Threading.Tasks;

namespace CAFFShop.Application.Services.Interfaces
{
	public interface IDownloadService
	{
		Task<Stream> GetFile(Guid animationId);
	}
}
