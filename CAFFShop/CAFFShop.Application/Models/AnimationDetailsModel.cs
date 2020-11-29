using System;
using System.Collections.Generic;
using System.Text;

namespace CAFFShop.Application.Models
{
    public class AnimationDetailsModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime CreationTime { get; set; }
        public string AuthorName { get; set; }
        public IEnumerable<CommentModel> Comments { get; set; }
        public bool CanDownloadCAFF { get; set; }
        public string PreviewFile { get; set; }
    }
}
