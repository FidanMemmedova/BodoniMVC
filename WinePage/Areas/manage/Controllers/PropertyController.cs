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
    public class PropertyController : Controller
    {
        private readonly BodoniDbContext _context;
        public PropertyController(BodoniDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index(int page = 1, int size = 5)
        {
            var query = _context.Properties;

            return View(PaginatedList<Property>.Create(query, page, size));
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Property property, IFormFile file)
        {
            if (!ModelState.IsValid)
                return View();

            if (_context.Properties.Any(x => x.Name == property.Name))
            {
                ModelState.AddModelError("Name", "The Property name is already taken.");
                return View();
            }

            _context.Properties.Add(property);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Property property = _context.Properties.Find(id);

            if (property == null)
                return View("Error");

            return View(property);
        }

        [HttpPost]
        public IActionResult Edit(Property property)
        {
            if (!ModelState.IsValid)
                return View();

            Property existProperty = _context.Properties.Find(property.Id);

            if (existProperty == null)
                return View("Error");

            if (property.Name != existProperty.Name && _context.Properties.Any(x => x.Id != property.Id && x.Name == property.Name))
            {
                ModelState.AddModelError("Name", "The Property name is already taken.");
                return View();
            }

            existProperty.Name = property.Name;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Property property = _context.Properties.Find(id);

            if (property == null)
                return View("Error");

            return View(property);
        }

        [HttpPost]
        public IActionResult Delete(Property property)
        {
            Property existProperty = _context.Properties.Find(property.Id);

            if (existProperty == null)
                return View("Error");

            _context.Properties.Remove(existProperty);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
