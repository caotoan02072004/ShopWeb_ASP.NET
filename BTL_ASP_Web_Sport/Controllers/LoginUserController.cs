using AspNetCoreHero.ToastNotification.Abstractions;
using BTL_ASP_Web_Sport.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BTL_ASP_Web_Sport.Controllers
{
    public class LoginUserController : Controller
    {

        private readonly ShopAppSportContext ctx;

        public LoginUserController(ShopAppSportContext context)
        {
            ctx = context;
        }

        public IActionResult LoginUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Dangnhap(string Email, string Password, string? returnUrl)
        {
            var acc = ctx.Accounts.FirstOrDefault(x => x.UserEmail == Email);
            if (acc != null)
            {
                if (acc.UserPassword == Password)
                {
                    if (acc.UserActive == 1)
                    {
                        if (acc.UserRole == 0)
                        {
                            HttpContext.Session.SetInt32("customerID", acc.UserId);
                            HttpContext.Session.SetString("customerName", acc.UserFullName);
                            HttpContext.Session.SetString("customerAvatar", acc.UserAvatar);
                            //_toastNotification.Success("Đăng nhập thành công !", 3);
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
                    return View("LoginUser", acc);
                }
            }
            else
            {
                ModelState.AddModelError("UserEmail", "Account does not exits!");
                return View("LoginUser", acc);
            }
            return View("LoginUser", acc);
        }

         public IActionResult Logout()
        {
            HttpContext.Session.Remove("customerID");
            HttpContext.Session.Remove("customerName");
            HttpContext.Session.Remove("customerAvatar");
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (login.IsCompletedSuccessfully)
            {
                return RedirectToAction("LoginUser", "LoginUser");
            }

            return RedirectToAction("Index", "Home");

        }
    }
}
