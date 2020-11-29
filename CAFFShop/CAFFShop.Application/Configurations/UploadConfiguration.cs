namespace CAFFShop.Application.Configurations
{
	public class UploadConfiguration
	{
		public string AnimationFormat { get; set; }
		public string PreviewFormat { get; set; }
		public int MaxUploadSizeBytes { get; set; }
		public int UploadTimeWindowLimitSeconds { get; set; }
		public int UploadCountLimitInTimeWindow { get; set; }
	}
}
