using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAFFShop.Dal;
using CAFFShop.Dal.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CAFFShop.Api.Pages.Animations
{
    public class AnimationDetailsModel : PageModel
    {
        private readonly CaffShopContext context;
        public string desc { get; set; }

        public AnimationDetailsModel(CaffShopContext context)
        {
            this.context = context;
        }

        public async Task OnGetAsync()
        {
            string id = (string)RouteData.Values["id"];
            var animation = await context.Animations.FindAsync(Guid.Parse(id));
            desc = animation.Description;

            

            
        }
    }
}
