using CAFFShop.Application.Models;
using CAFFShop.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CAFFShop.Application.Services.Interfaces
{
    public interface IReviewService
    {
        public Task<bool> Review(Guid animationId, ReviewState reviewState);

        public IEnumerable<AnimationReviewModel> LoadAnimations();
    }
}
