using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WinePage.DAL;
using WinePage.Models;

namespace WinePage.Areas.manage.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("manage")]

    public class SettingController : Controller
    {
        private readonly BodoniDbContext _context;
        public SettingController(BodoniDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = _context.Settings.ToDictionary(x => x.Key, y => y.Value);
            return View(model);
        }
        public IActionResult Edit(string id)
        {
            var existSetting = _context.Settings.FirstOrDefault(x => x.Key == id);

            if (existSetting == null)
                return View("error");

            return View(existSetting);
        }

        [HttpPost]
        public IActionResult Edit(Setting setting)
        {
            var existSetting = _context.Settings.FirstOrDefault(x => x.Key == setting.Key);

            if (existSetting == null)
                return View("error");

            existSetting.Value = setting.Value;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
