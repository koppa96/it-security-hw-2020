using System;

namespace CAFFShop.Dal.Abstractions
{
    public interface ICreationAuditedEntity
    {
        public DateTime CreationTime { get; set; }
    }
}