using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CAFFShop.Dal;
using CAFFShop.Dal.Entities;
using CAFFShop.Application.Services.Interfaces;
using CAFFShop.Application.Models;
using Microsoft.AspNetCore.Authorization;
using CAFFShop.Dal.Constants;
using CAFFShop.Api.Infrastructure.Filters;

namespace CAFFShop.Api.Pages.Users
{
    [Authorize(Roles = RoleTypes.Admin)]
    [LogRequestsFilter]
    public class IndexModel : PageModel
    {
        private readonly IUserService userService;

        public IndexModel(IUserService userService)
        {
            this.userService = userService;
        }

        [BindProperty]
        public List<UserListModel> Users { get;set; }

        public async Task OnGetAsync()
        {
            Users = await userService.ListUsersAsync();
        }

        public async Task OnPostAsync()
        {
            await userService.UpdateUserRolesAsync(Users);
            Users = await userService.ListUsersAsync();
        }
    }
}
