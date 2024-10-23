using AspNetCoreHero.ToastNotification.Abstractions;
using BTL_ASP_Web_Sport.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;
using X.PagedList;

namespace BTL_ASP_Web_Sport.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class AccountController : Controller
    {
        private readonly ShopAppSportContext ctx;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private readonly INotyfService _toastNotification;

        public AccountController(ShopAppSportContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment, INotyfService notyfService)
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
            var account = await ctx.Accounts.OrderByDescending(x => x.UserId).ToListAsync();
            if (!string.IsNullOrEmpty(name))
            {
                account = await ctx.Accounts.Where(x => x.UserFullName.Contains(name)).ToListAsync();
            }
            var pageData = account.ToPagedList(pageSize, limit);
            return View(pageData);
        }

        public IActionResult Detail(int id)
        {
            var account = ctx.Accounts.FirstOrDefault(a => a.UserId == id);

            if (account != null)
            {
                return View(account);
            }
            return NotFound();
        }

        public IActionResult Themmoi()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Luumoi(IFormFile fileUpload, Account data)
        {
            if (fileUpload != null)
            {
                var rootPath = _environment.ContentRootPath;
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(fileUpload.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("UserAvatar", "Hình ảnh không hợp lệ. Chỉ chấp nhận các định dạng: jpg, jpeg, png, gif.");
                }

                var path = Path.Combine(rootPath, "wwwroot", "Uploads", "accounts", fileUpload.FileName);

                using (var file = System.IO.File.Create(path))
                {
                    fileUpload.CopyTo(file);
                }
                data.UserAvatar = fileUpload.FileName;
            }

            ctx.Accounts.Add(data);
            ctx.SaveChanges();
            _toastNotification.Success("Thêm mới thành công !", 3);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Account account = ctx.Accounts.FirstOrDefault(a => a.UserId == id);
            if (account != null)
            {
                ctx.Accounts.Remove(account);
                ctx.SaveChanges();
            }
            _toastNotification.Success("Xóa thành công !", 3);
            return RedirectToAction("Index");
        }

        public IActionResult Sua(int id)
        {
            Account obj = ctx.Accounts.FirstOrDefault(a => a.UserId == id);
            return View(obj);
        }


        [HttpPost]
        public IActionResult Sua(int? id, string? oldImage, IFormFile fileUpload, Account data)
        {
            if (id != data.UserId)
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
                    ModelState.AddModelError("UserAvatar", "Hình ảnh không hợp lệ. Chỉ chấp nhận các định dạng: jpg, jpeg, png, gif.");
                }

                var path = Path.Combine(rootPath, "wwwroot", "Uploads", "accounts", fileUpload.FileName);

                if (!string.IsNullOrEmpty(oldImage))
                {
                    var pathOldFile = Path.Combine(rootPath, "wwwroot", "Uploads", "accounts", oldImage);
                    System.IO.File.Delete(pathOldFile);

                }

                using (var file = System.IO.File.Create(path))
                {
                    fileUpload.CopyTo(file);
                }

                data.UserAvatar = fileUpload.FileName;
            }
            else
            {
                data.UserAvatar = oldImage;
            }
            ctx.Accounts.Update(data);
            ctx.SaveChanges();
            _toastNotification.Success("Cập nhật thành công !", 3);
            return RedirectToAction("Index");
        }
    }
}
