using CAFFShop.Dal.Entities;
using System;
using System.Threading.Tasks;

namespace CAFFShop.Application.Services.Interfaces
{
    public interface IPurchaseService
    {
        public Task<Animation> GetAnimation(Guid animationId);

        public Task<bool> Purchase(Guid animationId, string billingAddress, string billingName);
    }
}
