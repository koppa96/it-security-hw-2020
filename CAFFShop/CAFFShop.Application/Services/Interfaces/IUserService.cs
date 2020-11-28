using CAFFShop.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAFFShop.Application.Services.Interfaces
{
    public interface IUserService
    {
        public Task<List<UserListModel>> ListUsersAsync();
        public Task UpdateUserRolesAsync(List<UserListModel> users);
    }
}
