using BTL_ASP_Web_Sport.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace BTL_ASP_Web_Sport.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly ShopAppSportContext _context;
        private readonly INotyfService _toastNotification;

        public LoginController(ShopAppSportContext context, INotyfService notyfService)
        {
            _context = context;
            _toastNotification = notyfService;
        }
        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        public IActionResult Dangnhap(string Email, string Password, string? returnUrl)
        {
            var acc = _context.Accounts.FirstOrDefault(x => x.UserEmail == Email);
            if (acc != null)
            {
                if (acc.UserPassword == Password)
                {
                    if (acc.UserActive == 1)
                    {
                        if (acc.UserRole == 1)
                        {
                            var identity = new ClaimsIdentity(new[]
                               {
                            new Claim("accId", acc.UserId.ToString()),
                            new Claim(ClaimTypes.Name, acc.UserName),
                            new Claim("userFullName", acc.UserFullName.ToString()),
                            new Claim(ClaimTypes.Email, acc.UserEmail),
                            new Claim("avatar", acc.UserAvatar ?? "default.png"),
                            new Claim(ClaimTypes.Role, acc.UserRole == 1 ? "Admin" : "User"),
                        }, CookieAuthenticationDefaults.AuthenticationScheme);
                            var principal = new ClaimsPrincipal(identity);
                            var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                            if (!string.IsNullOrEmpty(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            _toastNotification.Success("Đăng nhập thành công !", 3);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("UserEmail", "Do not Authorization!");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("UserEmail", "Account lockked!");
                    }
                }
                else
                {
                    ModelState.AddModelError("UserPassword", "Password incorrect !");
                    return View("Login", acc);
                }
            }
            else
            {
                ModelState.AddModelError("UserEmail", "Account does not exits!");
                return View("Login", acc);
            }
            return View("Login", acc);
        }

        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (login.IsCompletedSuccessfully)
            {
                return RedirectToAction("Index", "Login");
            }
            _toastNotification.Error("Đăng xuất thất bại !", 3);

            return RedirectToAction("Index", "Home");

        }
    }
}
