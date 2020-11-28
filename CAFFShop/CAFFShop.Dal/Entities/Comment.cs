using System;
using CAFFShop.Dal.Abstractions;

namespace CAFFShop.Dal.Entities
{
    public class Comment : ICreationAuditedEntity
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
        public DateTime CreationTime { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public Guid AnimationId { get; set; }
        public virtual Animation Animation { get; set; }
    }
}