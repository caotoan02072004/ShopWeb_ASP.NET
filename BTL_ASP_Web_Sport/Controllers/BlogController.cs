using BTL_ASP_Web_Sport.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_ASP_Web_Sport.Controllers
{
    public class BlogController : Controller
    {
        private readonly ShopAppSportContext ctx;

        public BlogController(ShopAppSportContext context)
        {
            ctx = context;
        }
        public IActionResult Index()
        {
            var Blogs = ctx.Blogs.OrderByDescending(x => x.BlogId).Take(9).ToList();
            ViewBag.Blogs = Blogs;
            return View();
        }

        public IActionResult DetailBlog(int id)
        {
            Blog obj = ctx.Blogs.FirstOrDefault(x => x.BlogId == id);
            if (obj != null)
            {
                return View(obj);
            }
            return NotFound();
        }
    }
}
