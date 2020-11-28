using CAFFShop.Application.Models;
using CAFFShop.Application.Services.Interfaces;
using CAFFShop.Dal;
using CAFFShop.Dal.Constants;
using CAFFShop.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAFFShop.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly CaffShopContext context;

        public UserService(UserManager<User> userManager, CaffShopContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public async Task<List<UserListModel>> ListUsersAsync()
        {
            var users = await userManager.Users.ToListAsync();
            var adminUsers = await userManager.GetUsersInRoleAsync(RoleTypes.Admin);

            return users.Select(x => new UserListModel
            {
                Id = x.Id,
                Email = x.Email,
                TwoFactorEnabled = x.TwoFactorEnabled,
                IsAdmin = adminUsers.Any(au => au.Id == x.Id),
                IsActive = x.IsActive
            }).ToList();
        }

        public async Task UpdateUserRolesAsync(List<UserListModel> users)
        {
            var usersInDb = await userManager.Users.ToListAsync();
            var adminUsers = await userManager.GetUsersInRoleAsync(RoleTypes.Admin);

            foreach (var user in usersInDb)
            {
                var updatedUser = users.SingleOrDefault(x => x.Id == user.Id);
                if (updatedUser != null)
                {
                    if (updatedUser.IsAdmin && adminUsers.All(x => x.Id != updatedUser.Id))
                    {
                        await userManager.AddToRoleAsync(user, RoleTypes.Admin);
                    }
                    else if (!updatedUser.IsAdmin && adminUsers.Any(x => x.Id == updatedUser.Id))
                    {
                        await userManager.RemoveFromRoleAsync(user, RoleTypes.Admin);
                    }

                    user.IsActive = updatedUser.IsActive;
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
