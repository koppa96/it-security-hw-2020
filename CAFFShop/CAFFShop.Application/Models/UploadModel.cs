﻿using System;

namespace CAFFShop.Application.Models
{
	public class UploadModel
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int Price { get; set; }
		public byte[] File { get; set; }
		public Guid UserId { get; set; }
	}
}
