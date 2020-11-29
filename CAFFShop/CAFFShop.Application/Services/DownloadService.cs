using CAFFShop.Application.Configurations;
using CAFFShop.Application.Services.Interfaces;
using CAFFShop.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CAFFShop.Application.Services
{
	public class DownloadService : IDownloadService
	{
		private CaffShopContext DbContext { get; }
		private StorageConfiguration StorageConfig { get; }
		private ILogger<DownloadService> Logger { get; }
		private ICanDownloadService CanDownloadService { get; }
		public DownloadService(CaffShopContext dbContext, IOptions<StorageConfiguration> storageConfiguration, ILogger<DownloadService> logger, ICanDownloadService canDownloadService)
		{
			this.DbContext = dbContext;
			this.StorageConfig = storageConfiguration.Value;
			this.Logger = logger;
			this.CanDownloadService = canDownloadService;
		}

		public async Task<Stream> GetFile(Guid animationId)
		{
			var animation = await DbContext.Animations.Include(a => a.File).SingleOrDefaultAsync(a => a.Id == animationId);

			if(!(await CanDownloadService.CanDownload(animation)))
			{
				Logger.LogError($"Unauthorized download attempt (Animation: {animationId})");
				return null;
			}

			FileStream fs;
			try
			{
				fs = File.OpenRead($"{StorageConfig.AnimationStorePath}/{animation.File.Path}");
			} catch(Exception e)
			{
				Logger.LogError(e, $"Failed file read (Animation: {animationId})");
				return null;
			}
			Logger.LogInformation($"Successful download (Animation: {animationId})");
			return fs;
		}
	}
}
