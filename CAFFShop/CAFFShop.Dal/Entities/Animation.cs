using System;
using System.Collections.Generic;
using CAFFShop.Dal.Abstractions;

namespace CAFFShop.Dal.Entities
{
    public class Animation : ICreationAuditedEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime CreationTime { get; set; }
        
        public Guid FileId { get; set; }
        public virtual File File { get; set; }

        public Guid? PreviewId { get; set; }
        public virtual File Preview { get; set; }

        public Guid? AuthorId { get; set; }
        public virtual User Author { get; set; }

        public ReviewState ReviewState { get; set; } = ReviewState.Pending;
        public Guid? ReviewedById { get; set; }
        public virtual User ReviewedBy { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<AnimationPurchase> AnimationPurchases { get; set; }
    }

    public enum ReviewState
    {
        Pending, Approved, Rejected
    }
}