using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WinePage.Areas.manage.ViewModels;
using WinePage.DAL;
using WinePage.Models;

namespace Pustok.Areas.manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly BodoniDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(BodoniDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Create()
        {
            //    AppUser appUser = new AppUser
            //    {
            //        UserName = "SuperAdmin",
            //        IsAdmin = true
            //    };

            //    var result = await _userManager.CreateAsync(appUser, "super123");

            //    if (!result.Succeeded)
            //    {
            //        return Ok(result.Errors);
            //    }

            //    await _userManager.AddToRoleAsync(appUser, "SuperAdmin");

            AppUser appUser = new AppUser
            {
                UserName = "Admin",
                IsAdmin = true
            };

            var result = await _userManager.CreateAsync(appUser, "admin123");

            if (!result.Succeeded)
            {
                return Ok(result.Errors);
            }

            await _userManager.AddToRoleAsync(appUser, "Admin");
            return Content("CREATED");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel admin, string returnUrl)
        {
            AppUser user = await _userManager.FindByNameAsync(admin.UserName);

            if (user == null || !user.IsAdmin)
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, admin.Password, false, true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View();
            }


            return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("index", "dashboard");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
