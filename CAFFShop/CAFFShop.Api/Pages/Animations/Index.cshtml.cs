using CAFFShop.Application.Models;
using CAFFShop.Dal;
using CAFFShop.Dal.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAFFShop.Application.Services.Interfaces;

namespace CAFFShop.Api.Pages.Animations
{
    public class IndexModel : PageModel
    {
        private readonly IAnimationListService animationListService;

        public IndexModel(IAnimationListService animationListService)
        {
            this.animationListService = animationListService;
        }

        public IList<AnimationListModel> Animation { get;set; }

        public async Task OnGetAsync()
        {
            Animation = await animationListService.ListAnimations();
        }
    }

}
