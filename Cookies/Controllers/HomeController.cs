using Cookies.Data;
using Cookies.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Cookies.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
           


            //if (Request.Cookies.TryGetValue("MyCookie", out string stringListSerialize))
            //{
            //    List<string> list = JsonConvert.DeserializeObject<List<string>>(stringListSerialize);
            //    ViewBag.list = list;
            //}
            //else
            //{
            //    List<string> stringList = new List<string>() { "1bG67", "R5kJi8", "gH4pOliJ" };
            //    string stringListSerialized = JsonConvert.SerializeObject(stringList);

            //    var cookieOptions = new CookieOptions
            //    {
            //        Expires = DateTime.Now.AddDays(1),
            //    };
            //    Response.Cookies.Append("MyCookie", stringListSerialized, cookieOptions);
            //}

            
            return View();

        }


        public async Task<IActionResult> Products()
        {
            var listProducts = _context.Products.ToList();
            var user = await _userManager.GetUserAsync(User);
            //ViewBag.userId = user.Id;

            if (Request.Cookies.TryGetValue("MyCookie", out string stringListSerialize))
            {
                List<string> list = JsonConvert.DeserializeObject<List<string>>(stringListSerialize);
                ViewBag.list = list;
            }

            return View(listProducts);
        }

        [HttpPost]
        public async Task<IActionResult> Products(ProductsModel pm, CartModel cm)
        {
            cm.ProductID = pm.ProductID;
            cm.ID = Guid.NewGuid().ToString();
            var user = await _userManager.GetUserAsync(User);
            

            if(user == null)
            {
                List<string> products;
                string stringListSerialized;
                // Check if the cookie already exists
                if (Request.Cookies["MyCookie"] != null)
                {
                     stringListSerialized = Request.Cookies["MyCookie"];
                    products = JsonConvert.DeserializeObject<List<string>>(stringListSerialized);
                }
                else
                {
                    products = new List<string>();
                }

                products.Add(pm.ProductID);
                 stringListSerialized = JsonConvert.SerializeObject(products);

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1)
                };

                Response.Cookies.Append("MyCookie", stringListSerialized, cookieOptions);

                
            }
            else
            {
                cm.UserID = user.Id;
                var doesItemExistInCart = _context.Cart.Where(x => x.ProductID == cm.ProductID && x.UserID == cm.UserID).Any();
                var getItemExistInCart = _context.Cart.Where(x => x.ProductID == cm.ProductID && x.UserID == cm.UserID).FirstOrDefault();

                if (doesItemExistInCart)
                {
                    getItemExistInCart.Quantity += 1;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    cm.Quantity = 1;
                    _context.Cart.Add(cm);
                    await _context.SaveChangesAsync();
                }
                ViewBag.userId = user.Id;
                
            }

            if (Request.Cookies.TryGetValue("MyCookie", out string stringListSerialize))
            {
                List<string> list = JsonConvert.DeserializeObject<List<string>>(stringListSerialize);
                ViewBag.list = list;
            }


            var listProducts = _context.Products.ToList();
            return View(listProducts);




        }
        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<PartialViewResult> CartIconPartial()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            var numberOfItemInCart = _context.Cart.Where(x => x.UserID == userId).ToList();
            var cartQauntity = 0;
            foreach(var count in numberOfItemInCart)
            {
                cartQauntity += count.Quantity;
            }

            ViewBag.cartTotal = cartQauntity;

            
            
            return PartialView("_CartIcon");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
