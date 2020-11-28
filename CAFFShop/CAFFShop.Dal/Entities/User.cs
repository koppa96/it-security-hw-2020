using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CAFFShop.Dal.Entities
{
    public class User : IdentityUser<Guid>
    {
        public virtual ICollection<AnimationPurchase> AnimationPurchases { get; set; }
        public virtual ICollection<Animation> UploadedAnimations { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
