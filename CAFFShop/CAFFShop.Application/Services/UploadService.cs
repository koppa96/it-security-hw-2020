using CAFFShop.Application.Configurations;
using CAFFShop.Application.Dtos;
using CAFFShop.Application.Services.Interfaces;
using CAFFShop.Dal;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CAFFShop.Application.Services
{
	public class UploadService : IUploadService
	{
		private CaffShopContext DbContext { get; }

		[DllImport(@"..\parser_core.dll")]
		private static extern ulong ParseAnimation(byte[] in_buffer, ulong in_len, byte[] out_buffer, ulong out_len);

		private UploadConfiguration UploadConfig { get; }

		public UploadService(CaffShopContext dbContext, IOptions<UploadConfiguration> uploadConfiguration)
		{
			this.DbContext = dbContext;
			this.UploadConfig = uploadConfiguration.Value;
			Directory.CreateDirectory(UploadConfig.PreviewPath);
			Directory.CreateDirectory(UploadConfig.AnimationStorePath);
		}

		public async Task AddAnimation(UploadDto dto)
		{
			var previewId = await CreatePreview(dto.File);
			var animationFileId = await SaveAnimationFile(dto.File);

			DbContext.Animations.Add(new Dal.Entities.Animation
			{
				Id = Guid.NewGuid(),
				Name = dto.Name,
				Description = dto.Description,
				Price = dto.Price,
				CreationTime = DateTime.Now,
				FileId = animationFileId,
				PreviewId = previewId,
				AuthorId = dto.UserId
			});

			await DbContext.SaveChangesAsync();
		}

		private async Task<Guid> CreatePreview(byte[] file)
		{
			byte[] out_buff = new byte[file.Length];
			int real_out_len = (int)ParseAnimation(file, (ulong)file.Length + 1, out_buff, (ulong)out_buff.Length + 1);
			return await SaveFile(UploadConfig.PreviewPath, UploadConfig.PreviewFormat, out_buff);
		}

		private async Task<Guid> SaveAnimationFile(byte[] file)
		 => await SaveFile(UploadConfig.AnimationStorePath, UploadConfig.AnimationFormat, file);

		private async Task<Guid> SaveFile(string path, string fileExtension, byte[] content)
		{
			var id = Guid.NewGuid();
			var filePath = $"{path}/{id}.{fileExtension}";
			await File.WriteAllBytesAsync(filePath, content);

			DbContext.Files.Add(new Dal.Entities.File()
			{
				Id = id,
				Path = filePath
			});

			return id;
		}
	}
}
