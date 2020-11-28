using CAFFShop.Dal.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CAFFShop.Api.Pages.Animations
{
    public class PurchaseModel : PageModel
    {
        private readonly CAFFShop.Dal.CaffShopContext _context;

        public PurchaseModel(CAFFShop.Dal.CaffShopContext context)
        {
            _context = context;
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
            var userId = TryGetClaim();
            if(userId == null)
            {
                return RedirectToPage("/Animations/Index");
            }

            Animation = await _context.Animations
                .FirstOrDefaultAsync(a => a.Id == id);
            if(Animation == null)
            {
                return NotFound();
            }

            if(await _context.AnimationPurchases.AnyAsync(p => p.UserId == userId && p.Animation == Animation) || Animation.AuthorId == userId)
            {
                return RedirectToPage("/Animations/AnimationDetails", new { id });
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
