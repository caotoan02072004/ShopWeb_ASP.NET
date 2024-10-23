using AspNetCoreHero.ToastNotification.Abstractions;
using BTL_ASP_Web_Sport.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Microsoft.CodeAnalysis;

namespace BTL_ASP_Web_Sport.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ShopAppSportContext ctx;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private readonly INotyfService _toastNotification;

        public ProductController(ShopAppSportContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment, INotyfService notyfService)
        {
            ctx = context;  
            _environment = environment;
            _toastNotification = notyfService;
        }
        public async Task<IActionResult> Index(string? name, int? page = 1)
        {
            var pageSize = page ?? 1;
            var limit = 5;
            ViewBag.names = name;
            var listPro = await ctx.Products.Include(x => x.ProCategory).OrderByDescending(x => x.ProId).ToListAsync();
            if (!string.IsNullOrEmpty(name))
            {
                listPro = await ctx.Products.Where(x => x.ProName.Contains(name)).ToListAsync();
            }
            var pageData = listPro.ToPagedList(pageSize, limit);
            return View(pageData);
        }

        public IActionResult Chitietsanpham(int Id)
        {
            Product obj = ctx.Products.Include(x => x.ProCategory).FirstOrDefault(p => p.ProId == Id);

            if (obj != null)
            {
                return View(obj);
            }

            return NotFound();
        }

        public IActionResult Themmoi() 
        {
            ViewData["ProCategoryId"] = new SelectList(ctx.Categories, "CateId", "CateName");
            return View();
        }

        [HttpPost]
        public IActionResult Luumoi(IFormFile fileUpload, Product data)
        {
            if (fileUpload != null)
            {
                var rootPath = _environment.ContentRootPath;
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(fileUpload.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("BlogImage", "Hình ảnh không hợp lệ. Chỉ chấp nhận các định dạng: jpg, jpeg, png, gif.");
                }

                var path = Path.Combine(rootPath, "wwwroot", "Uploads", "products", fileUpload.FileName);

                using (var file = System.IO.File.Create(path))
                {
                    fileUpload.CopyTo(file);
                }
                data.ProImage = fileUpload.FileName;
            }
            ctx.Products.Add(data);
            ctx.SaveChanges();
            _toastNotification.Success("Thêm mới thành công !", 3);
            return RedirectToAction("Index");
        }



        public IActionResult Sua(int Id)
        {
            Product obj = ctx.Products.FirstOrDefault(p => p.ProId == Id);
            ViewData["ProCategoryId"] = new SelectList(ctx.Categories, "CateId", "CateName");
            return View(obj);
        }

        [HttpPost]
        public IActionResult Sua(int? id, string? oldImage, IFormFile fileUpload, Product data)
        {
            if (id != data.ProId)
            {
                return NotFound();
            }
            if (fileUpload != null)
            {
                var rootPath = _environment.ContentRootPath;
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(fileUpload.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("ProductImage", "Hình ảnh không hợp lệ. Chỉ chấp nhận các định dạng: jpg, jpeg, png, gif.");
                }

                var path = Path.Combine(rootPath, "wwwroot", "Uploads", "products", fileUpload.FileName);

                if (!string.IsNullOrEmpty(oldImage))
                {
                    var pathOldFile = Path.Combine(rootPath, "wwwroot", "Uploads", "products", oldImage);
                    System.IO.File.Delete(pathOldFile);

                }

                using (var file = System.IO.File.Create(path))
                {
                    fileUpload.CopyTo(file);
                }

                data.ProImage = fileUpload.FileName;
            }
            else
            {
                data.ProImage = oldImage;
            }
            ctx.Products.Update(data);
            ctx.SaveChanges();
            _toastNotification.Success("Cập nhật thành công !", 3);
            return RedirectToAction("Index");
        }



        public IActionResult Xoa(int id) 
        {
            Product obj = ctx.Products.FirstOrDefault(p => p.ProId == id);
            if(obj != null)
            {
                ctx.Products.Remove(obj);
                ctx.SaveChanges();
            }
            _toastNotification.Success("Xóa thành công !", 3);
            return RedirectToAction("Index");
        }
    }
}
