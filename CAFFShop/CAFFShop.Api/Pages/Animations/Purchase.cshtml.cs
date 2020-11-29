using CAFFShop.Api.Infrastructure.Filters;
using CAFFShop.Application.Services.Interfaces;
using CAFFShop.Dal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CAFFShop.Api.Pages.Animations
{
    [Authorize]
    [LogRequestsFilter]
    public class PurchaseModel : PageModel
    {

        private readonly IPurchaseService purchaseService;

        public PurchaseModel(IPurchaseService purchaseService)
        {

            this.purchaseService = purchaseService;
        }

        public AnimationPurchase AnimationPurchase { get; set; }
        public Animation Animation { get; set; }

        [BindProperty, Required, Display(Name = "Számlázási név"), MaxLength(100)]
        public string BillingAddress { get; set; }
        [BindProperty, Required, Display(Name = "Számlázási cím"), MaxLength(100)]
        public string BillingName { get; set; }
        [BindProperty]
        public bool TosAccepted { get; set; }
        [BindProperty]
        public bool Newsletter { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {

            Animation = await purchaseService.GetAnimation(id.GetValueOrDefault());
            if (Animation == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<ActionResult> OnPostAsync(Guid? id)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (!TosAccepted)
            {
                return Page();
            }

            if (await purchaseService.Purchase(id.GetValueOrDefault(), BillingAddress, BillingName))
            {
                return RedirectToPage("/Animations/Index");
            }
            else 
            {
                return NotFound();
            }
            
        }
    }
}
