using Cookies.Data;
using Cookies.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookies.Controllers
{
    public class CartController: Controller
    {
        private readonly ILogger<CartController> _logger;
        ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public CartController(ILogger<CartController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> MyCart()
        {
            var user = await _userManager.GetUserAsync(User);
            List<MyCartModel> myCart = _context.Cart.Where(o => o.UserID == user.Id).Join(_context.Products, cart => cart.ProductID, product => product.ProductID, (cart, product) => new { cart, product }).Select(s => new MyCartModel { Name = s.product.Name, Description = s.product.Description, Image = s.product.Image, Quantity = s.cart.Quantity }).ToList();
            return View(myCart);
        }
    }
}
