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
    public class BrandController : Controller
    {
        private readonly BodoniDbContext _context;
        public BrandController(BodoniDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index(int page = 1, int size = 5)
        {
            var query = _context.Brands;

            return View(PaginatedList<Brand>.Create(query, page, size));
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Brand brand, IFormFile file)
        {
            if (!ModelState.IsValid)
                return View();

            if (_context.Brands.Any(x => x.Name == brand.Name))
            {
                ModelState.AddModelError("Name", "The Brand name is already taken.");
                return View();
            }

            _context.Brands.Add(brand);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Brand brand = _context.Brands.Find(id);

            if (brand == null)
                return View("Error");

            return View(brand);
        }

        [HttpPost]
        public IActionResult Edit(Brand brand)
        {
            if (!ModelState.IsValid)
                return View();

            Brand existBrand = _context.Brands.Find(brand.Id);

            if (existBrand == null)
                return View("Error");

            if (brand.Name != existBrand.Name && _context.Brands.Any(x => x.Id != brand.Id && x.Name == brand.Name))
            {
                ModelState.AddModelError("Name", "The Brand name is already taken.");
                return View();
            }

            existBrand.Name = brand.Name;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Brand brand = _context.Brands.Find(id);

            if (brand == null)
                return View("Error");

            return View(brand);
        }

        [HttpPost]
        public IActionResult Delete(Brand brand)
        {
            Brand existBrand = _context.Brands.Find(brand.Id);

            if (existBrand == null)
                return View("Error");

            _context.Brands.Remove(existBrand);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
