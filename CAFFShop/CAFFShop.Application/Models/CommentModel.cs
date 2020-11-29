using System;
using System.Collections.Generic;
using System.Text;

namespace CAFFShop.Application.Models
{
    public class CommentModel
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
        public DateTime CreationTime { get; set; }

        public Guid UserId { get; set; }
        public string UserName { get; set; }

    }

}
