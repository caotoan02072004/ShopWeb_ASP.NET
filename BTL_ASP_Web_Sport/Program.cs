
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using BTL_ASP_Web_Sport.Data;
using AspNetCoreHero.ToastNotification;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages().AddNToastNotifyNoty(new NotyOptions
{
    ProgressBar = true,
    Timeout = 5000
});
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
    config.HasRippleEffect = true;
});


// Add Razor Options Service
builder.Services.AddControllersWithViews()
    .AddRazorOptions(opts =>
    {
        opts.ViewLocationFormats.Add("/{0}.cshtml");
    });

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".app.session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Authentication - Authorize Servicess
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    //options.AccessDeniedPath = new PathString("/Manager/Home/Index");
    options.Cookie = new CookieBuilder
    {
        HttpOnly = true,
        Name = "app.cookie",
        Path = "/",
        SameSite = SameSiteMode.Lax,
        SecurePolicy = CookieSecurePolicy.SameAsRequest
    };
    options.LoginPath = "/Admin/Login";
    options.ReturnUrlParameter = "returnUrl";
    options.SlidingExpiration = true;
});

// Configure the connect to MSSQL 
builder.Services.AddDbContext<ShopAppSportContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: options => { options.EnableRetryOnFailure(); }));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();

// Using cookie and authen -author in this app
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Add Controller Route in area Admin
app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();