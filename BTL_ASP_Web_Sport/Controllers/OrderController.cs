using AspNetCoreHero.ToastNotification.Abstractions;
using BTL_ASP_Web_Sport.Data;
using Microsoft.AspNetCore.Mvc;

namespace BTL_ASP_Web_Sport.Controllers
{
    public class OrderController : Controller
    {
        private readonly ShopAppSportContext ctx;

        public OrderController(ShopAppSportContext context)
        {
            ctx = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
