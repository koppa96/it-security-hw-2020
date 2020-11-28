using CAFFShop.Application.Configurations;
using CAFFShop.Application.Dtos;
using CAFFShop.Application.Services.Interfaces;
using CAFFShop.Dal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

		public async Task<List<string>> AddAnimation(UploadDto dto)
		{
			var errors = CheckUploadRequirements(dto);
			if (errors != null && errors.Count > 0)
				return errors;

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
			return null;
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
			var fileName = $"{id}.{fileExtension}";
			var filePath = $"{path}/{fileName}";
			await File.WriteAllBytesAsync(filePath, content);

			DbContext.Files.Add(new Dal.Entities.File()
			{
				Id = id,
				Path = fileName
			});

			return id;
		}

		private List<string> CheckUploadRequirements(UploadDto dto)
		{
			var errors = new List<string>();
			if (dto.File.Length > UploadConfig.MaxUploadSizeBytes)
				errors.Add("Túl nagy feltöltött fájl!");

			var timeLimitWindowStart = DateTime.Now - TimeSpan.FromSeconds(UploadConfig.UploadTimeWindowLimitSeconds);
			var lastUploadsCount = DbContext.Animations.Count(a => a.CreationTime > timeLimitWindowStart && a.AuthorId == dto.UserId);
			if (lastUploadsCount > UploadConfig.UploadCountLimitInTimeWindow)
				errors.Add("Túl sok fájlt töltött fel! Próbálja meg később!");

			return errors;
		}
	}
}
