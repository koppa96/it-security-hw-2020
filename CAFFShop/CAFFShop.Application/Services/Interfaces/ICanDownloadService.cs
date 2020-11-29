using CAFFShop.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CAFFShop.Application.Services.Interfaces
{
    public interface ICanDownloadService
    {
        public Task<bool> CanDownload(Animation animation);
    }
}
