using BTL_ASP_Web_Sport.Data;
using BTL_ASP_Web_Sport.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BTL_ASP_Web_Sport.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShopAppSportContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ShopAppSportContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var Blogs = _context.Blogs.OrderByDescending(x => x.BlogId).Take(3);
            var ProSale = _context.Products.Where(x => x.ProSalePrice > 0).Take(8);
            var NewPro = _context.Products.OrderByDescending(x => x.ProId).Take(8);
            ViewBag.Blogs = Blogs;
            ViewBag.ProSale = ProSale;
            ViewBag.NewPro = NewPro;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
