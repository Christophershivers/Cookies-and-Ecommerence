using Cookies.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookies.ViewComponents
{
    public class CartIconViewComponent: ViewComponent
    {
        private readonly ILogger<CartIconViewComponent> _logger;
        ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public CartIconViewComponent(ILogger<CartIconViewComponent> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).GetAwaiter().GetResult();
            var userId = user.Id;
            var numberOfItemInCart = _context.Cart.Where(x => x.UserID == userId).ToList();
            var cartQauntity = 0;
            foreach (var count in numberOfItemInCart)
            {
                cartQauntity += count.Quantity;
            }

            ViewBag.cartTotal = cartQauntity;


            var listItemsInCart = _context.Cart.Where(x => x.UserID == userId).ToList();
            return View(listItemsInCart);
        }
    }
}
