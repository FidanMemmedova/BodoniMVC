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
    public class CategoryController : Controller
    {
        private readonly BodoniDbContext _context;
        public CategoryController(BodoniDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index(int page = 1, int size = 5)
        {
            var query = _context.Categories;

            return View(PaginatedList<Category>.Create(query, page, size));
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category, IFormFile file)
        {
            if (!ModelState.IsValid)
                return View();

            if (_context.Categories.Any(x => x.Name == category.Name))
            {
                ModelState.AddModelError("Name", "The Category name is already taken.");
                return View();
            }

            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Category category = _context.Categories.Find(id);

            if (category == null)
                return View("Error");

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
                return View();

            Category existCategory = _context.Categories.Find(category.Id);

            if (existCategory == null)
                return View("Error");

            if (category.Name != existCategory.Name && _context.Categories.Any(x => x.Id != category.Id && x.Name == category.Name))
            {
                ModelState.AddModelError("Name", "The category name is already taken.");
                return View();
            }

            existCategory.Name = category.Name;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Category category = _context.Categories.Find(id);

            if (category == null)
                return View("Error");

            return View(category);
        }

        [HttpPost]
        public IActionResult Delete(Category category)
        {
            Category existCategory = _context.Categories.Find(category.Id);

            if (existCategory == null)
                return View("Error");

            _context.Categories.Remove(existCategory);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
