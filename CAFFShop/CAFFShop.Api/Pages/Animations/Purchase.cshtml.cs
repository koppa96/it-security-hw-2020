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
        private readonly CAFFShop.Dal.CaffShopContext _context;
        private readonly IIdentityService identityService;
        private readonly IPurchaseService purchaseService;

        public PurchaseModel(CAFFShop.Dal.CaffShopContext context, IIdentityService identityService, IPurchaseService purchaseService)
        {
            _context = context;
            this.identityService = identityService;
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

        private Guid? TryGetClaim()
        {
            var claim = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                return Guid.Parse(claim.Value);
            }
            return null;
        }

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
            var userId = TryGetClaim();
            if (userId == null)
            {
                return RedirectToPage("/Animations/Index");
            }

            Animation = await _context.Animations
                .Include(a => a.Preview)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (Animation == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (!TosAccepted)
            {
                return Page();
            }

            AnimationPurchase = new AnimationPurchase()
            {
                Id = Guid.NewGuid(),
                Animation = Animation,
                UserId = userId.Value,
                BillingAddress = BillingAddress,
                BillingName = BillingName,
                PriceAtPurchase = Animation.Price,
                CreationTime = DateTime.Now
            };
            _context.AnimationPurchases.Add(AnimationPurchase);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Animations/Index");
        }
    }
}
