using CAFFShop.Api.Infrastructure.Filters;
using CAFFShop.Application.Models;
using CAFFShop.Application.Services.Interfaces;
using CAFFShop.Dal.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<IActionResult> OnPostAsync()
        {
            await userService.UpdateUserRolesAsync(Users);
            return RedirectToPage();
        }
    }
}
