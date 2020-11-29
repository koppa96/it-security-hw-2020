using System;
using System.Collections.Generic;
using System.Text;

namespace CAFFShop.Application.Models
{
    public class AnimationListModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreationDate { get; set; }
        public string AuthorName { get; set; }
        public int NumberOfPurchases { get; set; }
        public int NumberOfComments { get; set; }
        public int Price { get; set; }
        public bool Own { get; set; }
        public bool HasPurchased { get; set; }
        public string Preview { get; set; }
    }
}
