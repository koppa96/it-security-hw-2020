using System;
using CAFFShop.Dal.Abstractions;

namespace CAFFShop.Dal.Entities
{
    public class AnimationPurchase : ICreationAuditedEntity
    {
        public Guid Id { get; set; }

        public string BillingName { get; set; }
        public string BillingAddress { get; set; }
        
        public DateTime CreationTime { get; set; }
        public int PriceAtPurchase { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public Guid AnimationId { get; set; }
        public virtual Animation Animation { get; set; }
    }
}
