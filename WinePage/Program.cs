using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WinePage.DAL;
using WinePage.Models;
using WinePage.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BodoniDbContext>(opt =>
{
    opt.UseSqlServer("Server=DESKTOP-U9J9LCG\\SQLEXPRESS;Database=BodoniDb;Trusted_Connection=True");
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
    opt.Lockout.MaxFailedAccessAttempts = 3;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
}).AddDefaultTokenProviders().AddEntityFrameworkStores<BodoniDbContext>();

builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromSeconds(5);
});
builder.Services.AddScoped<LayoutService>();
builder.Services.AddCookiePolicy(opts =>
{
    opts.OnAppendCookie = ctx =>
    {
        ctx.CookieOptions.Expires = DateTimeOffset.UtcNow.AddDays(14);
    };
});

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.Events.OnRedirectToLogin = opt.Events.OnRedirectToAccessDenied = context =>
    {
        if (context.HttpContext.Request.Path.Value.StartsWith("/manage"))
        {
            var uri = new Uri(context.RedirectUri);
            context.Response.Redirect("/manage/account/login" + uri.Query);
        }
        else
        {
            var uri = new Uri(context.RedirectUri);
            context.Response.Redirect("/account/login" + uri.Query);
        }

        return Task.CompletedTask;
    };
});



var app = builder.Build();
app.UseStaticFiles();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
          );
app.MapControllerRoute("default", "{controller=home}/{action=Index}/{id?}");
app.Run();
