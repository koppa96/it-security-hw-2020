using System;
using System.Collections.Generic;
using System.Text;

namespace CAFFShop.Application.Services.Interfaces
{
    public interface IIdentityService
    {
        public Guid? GetUserId();

        public bool IsAdmin();

        public bool IsAuthenticated();
    }
}
