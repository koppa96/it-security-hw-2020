using CAFFShop.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CAFFShop.Application.Services.Interfaces
{
    public interface IDetailsService
    {
        public Task<AnimationDetailsModel> GetAnimationDetails(Guid id);

        public Task DeleteComment(Guid commentId);

        public Task CreateComment(Guid animationId, string text);
    }
}
