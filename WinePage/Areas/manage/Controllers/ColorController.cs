using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WinePage.DAL;
using WinePage.Models;
using WinePage.ViewModels;

namespace WinePage.Areas.manage.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("manage")]
    public class ColorController : Controller
    {
        private readonly BodoniDbContext _context;
        public ColorController(BodoniDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1,int size=5)
        {
            var query = _context.Colors;

            return View(PaginatedList<Color>.Create(query, page, size));
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Color color, IFormFile file)
        {
            if (!ModelState.IsValid)
                return View();

            if (_context.Colors.Any(x => x.Name == color.Name))
            {
                ModelState.AddModelError("Name", "The Color name is already taken.");
                return View();
            }

            _context.Colors.Add(color);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Color color = _context.Colors.Find(id);

            if (color == null)
                return View("Error");

            return View(color);
        }

        [HttpPost]
        public IActionResult Delete(Color color)
        {
            Color existColor = _context.Colors.Find(color.Id);

            if (existColor == null)
                return View("Error");

            _context.Colors.Remove(existColor);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
