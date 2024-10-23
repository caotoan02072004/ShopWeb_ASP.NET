using BTL_ASP_Web_Sport.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace BTL_ASP_Web_Sport.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BlogController : Controller
    {
        private readonly ShopAppSportContext ctx;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private readonly INotyfService _toastNotification;

        public BlogController(ShopAppSportContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment, INotyfService notyfService)
        {
            ctx = context;
            _environment = environment;
            _toastNotification = notyfService;
        }
        public async Task<IActionResult> Index(string? name ,int? page = 1)
        {
            var pageSize = page ?? 1;
            var limit = 5;
            ViewBag.names = name;
            var blogs = await ctx.Blogs.OrderByDescending(x => x.BlogId).ToListAsync();
            if (!string.IsNullOrEmpty(name))
            {
                blogs = await ctx.Blogs.Where(x => x.BlogName.Contains(name)).ToListAsync();
            }
            var pageData = blogs.ToPagedList(pageSize, limit);
            return View(pageData);
        }

        public IActionResult Themmoi()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(IFormFile fileUpload, Blog data) 
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

                var path = Path.Combine(rootPath, "wwwroot", "Uploads", "blogs", fileUpload.FileName);

                using (var file = System.IO.File.Create(path))
                {
                    fileUpload.CopyTo(file);
                }
                data.BlogImage = fileUpload.FileName;
            }
            ctx.Blogs.Add(data);
            ctx.SaveChanges();
            _toastNotification.Success("Thêm mới thành công !", 3);
            return RedirectToAction("Index");
        } 

        public IActionResult Xoa(int Id)
        {
            Blog obj = ctx.Blogs.FirstOrDefault(p => p.BlogId == Id);
            if (obj != null)
            {
                ctx.Blogs.Remove(obj);
                ctx.SaveChanges();
            }
            _toastNotification.Success("Xóa thành công !", 3);
            return RedirectToAction("Index");
        }

        public IActionResult Sua(int Id)
        {
            Blog obj = ctx.Blogs.FirstOrDefault(b => b.BlogId == Id);
            return View(obj);
        }

        [HttpPost]
        public IActionResult Sua(int? id, string? oldImage, IFormFile fileUpload, Blog data)
        {
            if (id != data.BlogId)
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
                    ModelState.AddModelError("BlogImage", "Hình ảnh không hợp lệ. Chỉ chấp nhận các định dạng: jpg, jpeg, png, gif.");
                }

                var path = Path.Combine(rootPath, "wwwroot", "Uploads", "blogs", fileUpload.FileName);

                if (!string.IsNullOrEmpty(oldImage))
                {
                    var pathOldFile = Path.Combine(rootPath, "wwwroot", "Uploads", "blogs", oldImage);
                    System.IO.File.Delete(pathOldFile);

                }

                using (var file = System.IO.File.Create(path))
                {
                    fileUpload.CopyTo(file);
                }

                data.BlogImage = fileUpload.FileName;
            }
            else
            {
                data.BlogImage = oldImage;
            }
            ctx.Blogs.Update(data);
            ctx.SaveChanges();
            _toastNotification.Success("Cập nhật thành công !", 3);
            return RedirectToAction("Index");
        }
    }
}
