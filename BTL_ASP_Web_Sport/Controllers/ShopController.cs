using AspNetCoreHero.ToastNotification.Abstractions;
using BTL_ASP_Web_Sport.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using X.PagedList;


namespace BTL_ASP_Web_Sport.Controllers
{
    public class ShopController : Controller
    {
        private readonly ShopAppSportContext ctx;
        private readonly INotyfService _toastNotification;

        public ShopController(ShopAppSportContext context, INotyfService notyfService)
        {
            ctx = context;
            _toastNotification = notyfService;
        }


        public async Task<IActionResult> Index(string price, string name, int? page = 1, double? minPrice = 0, double? maxPrice = 0, int? id = 0)
        {
            var pageSize = page ?? 1;
            var limit = 6;
            ViewBag.names = name;
            ViewBag.Price = price;
            var AllCate = await ctx.Categories.OrderByDescending(x => x.CateId).ToListAsync();
            ViewBag.allCate = AllCate;

            var listPro = await ctx.Products.Include(x => x.ProCategory).OrderByDescending(x => x.ProId).ToListAsync();
            if (!string.IsNullOrEmpty(name))
            {
                listPro = await ctx.Products.Where(x => x.ProName.Contains(name)).ToListAsync();
            }
            if (minPrice >= 0 && maxPrice > 0)
            {
                listPro = await ctx.Products.Where(p => p.ProPrice >= minPrice && p.ProPrice <= maxPrice).ToListAsync();
            }
            if (price == "minPrice")
            {
                listPro = await ctx.Products.OrderBy(x => x.ProPrice).ToListAsync();
            }
            else if (price == "maxPrice")
            {
                listPro = await ctx.Products.OrderByDescending(x => x.ProPrice).ToListAsync();
            }

            if (id != null && id > 0)
            {
                listPro = await ctx.Products.Where(x => x.ProCategoryId == id).ToListAsync();
            }
            var pageData = listPro.ToPagedList(pageSize, limit);
            return View(pageData);
        }

        public IActionResult Detail(int Id)
        {
            Product obj = ctx.Products.FirstOrDefault(x => x.ProId == Id);
            if (obj != null)
            {
                return View(obj);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult AddToCart(int quantity, int ProductId)
        {
            var userId = HttpContext.Session.GetInt32("customerID");

            var existingCartItem = ctx.Carts
                .FirstOrDefault(c => c.UserId == userId && c.ProductId == ProductId);
            var product = ctx.Products.Find(ProductId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;
                existingCartItem.TotalAmount = Convert.ToInt32(existingCartItem.Quantity * product.ProPrice);
                // Cập nhật vào database
                ctx.Carts.Update(existingCartItem);
            }
            else
            {
                Cart cart = new Cart
                {
                    UserId = userId,
                    ProductId = ProductId,
                    Quantity = quantity,
                    TotalAmount = Convert.ToInt32(quantity * product.ProPrice)
                };
                ctx.Carts.Add(cart);
            }
            ctx.SaveChanges();
            _toastNotification.Success("Thêm giỏ hàng thành công !", 3);
            return RedirectToAction("Detail", new { id = ProductId });
        }

        public async Task<IActionResult> listCart()
        {
            var userId = HttpContext.Session.GetInt32("customerID");
            var listCart = await ctx.Carts.Include(x => x.Product).OrderByDescending(x => x.CartId).Where(x => x.UserId == userId).ToListAsync();
            ViewBag.Cart = listCart;
            return View();
        }

        public IActionResult DeleteCart(int id)
        {
            Cart obj = ctx.Carts.FirstOrDefault(c => c.CartId == id);
            if (obj != null)
            {
                ctx.Carts.Remove(obj);
                ctx.SaveChanges();
            }
            _toastNotification.Success("Xóa sản phẩm trong giỏ hàng thành công !", 3);
            return RedirectToAction("listCart");
        }
    }
}
