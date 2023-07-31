using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WinePage.DAL;
using WinePage.Helpers;
using WinePage.Models;
using WinePage.ViewModels;
using WinePage.ViewModels.Wine;

namespace WinePage.Areas.manage.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("manage")]
    public class WineController : Controller
    {
        private readonly BodoniDbContext _context;
        private readonly IWebHostEnvironment _env;

        public WineController(BodoniDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1, int size = 5)
        {
            var allWine = _context.Wine
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.Property)
                .Include(x => x.WineColors).ThenInclude(a => a.color)
                .Where(x => !x.IsDeleted).ToList();

            List<Wine_VM> allWine_VM = new();
            foreach (var wine in allWine)
            {
                Wine_VM wine_VM = new()
                {
                    Id= wine.Id,    
                    Name= wine.Name,
                    Brand=wine.Brand.Name,
                    Image= wine.Image
                };
                allWine_VM.Add(wine_VM);
            }
            return View(PaginatedList<Wine_VM>.Create(allWine_VM.AsQueryable(), page, size));
        }

        public IActionResult Create()
        {
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Properties = _context.Properties.ToList();
            ViewBag.Colors = _context.Colors.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Wine wine)
        {
            CheckCreate(wine);

            if (!ModelState.IsValid)
            {
                ViewBag.Brands = _context.Brands.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Properties = _context.Properties.ToList();
                ViewBag.Colors = _context.Colors.ToList();

                return View();
            }

            wine.WineColors = wine.ColorIds.Select(x => new WineColor { ColorId = x }).ToList();

            if (wine.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image is required");
                return View();
            }

            if (wine.ImageFile.ContentType != "image/jpeg" && wine.ImageFile.ContentType != "image/png")
                ModelState.AddModelError("ImageFile", "ImageFile must be image/png or image/jpeg");

            if (wine.ImageFile.Length > 2097152)
                ModelState.AddModelError("ImageFile", "ImageFile must be less or equal than 2MB");

            wine.Image = FileManager.Save(wine.ImageFile, _env.WebRootPath + "/uploads/wineImages");

            wine.CreatedAt = DateTime.UtcNow;

            _context.Wine.Add(wine);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        private void CheckCreate(Wine wine)
        {
            if (!_context.Brands.Any(x => x.Id == wine.BrandId))
            {
                ModelState.AddModelError("BrandId", "Brand not found");
                return;
            }

            if (!_context.Categories.Any(x => x.Id == wine.CategoryId))
            {

                ModelState.AddModelError("CategoryId", "Category not found");
                return;
            } 
            if (!_context.Properties.Any(x => x.Id == wine.PropertyId))
            {

                ModelState.AddModelError("PropertyId", "Property not found");
                return;
            }

            if (wine.ImageFile == null)
                ModelState.AddModelError("ImageFile", "Image File is required");


            foreach (var colorId in wine.ColorIds)
            {
                if (!_context.Colors.Any(x => x.Id == colorId))
                {
                    ModelState.AddModelError("ColorIds", "Color not found");
                    break;
                }
            }
        }
        public IActionResult Edit(int id)
        {
            Wine wine = _context.Wine
                .Include(x => x.WineColors)
                .FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (wine == null)
                return View("Error");

            wine.ColorIds = wine.WineColors.Select(x => x.ColorId).ToList();

            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Properties = _context.Properties.ToList();
            ViewBag.Colors = _context.Colors.ToList();

            return View(wine);
        }

        [HttpPost]
        public IActionResult Edit(Wine wine)
        {
            if (!ModelState.IsValid) return View();

            Wine existWine = _context.Wine
                .Include(x => x.WineColors).FirstOrDefault(x => x.Id == wine.Id);

            if (existWine == null)
                return View("Error");

            CheckEdit(wine, existWine);
            existWine.Description= wine.Description;
            existWine.SalePrice = wine.SalePrice;
            existWine.DiscountPercent = wine.DiscountPercent;
            existWine.CostPrice = wine.CostPrice;
            existWine.Name = wine.Name;
            existWine.CategoryId = wine.CategoryId;
            existWine.BrandId = wine.BrandId;
            existWine.PropertyId = wine.PropertyId;


            var newWineColors = wine.ColorIds
                .FindAll(x => !existWine.WineColors.Any(bt => bt.ColorId == x))
                .Select(x => new WineColor { ColorId = x }).ToList();

            existWine.WineColors.RemoveAll(x => !wine.ColorIds.Contains(x.ColorId));
            existWine.WineColors.AddRange(newWineColors);

            if (wine.ImageFile != null)
            {
                string oldFileName = existWine.Image;
                existWine.Image = FileManager.Save(wine.ImageFile, _env.WebRootPath + "/uploads/wineImages");
                FileManager.Delete(_env.WebRootPath + "/uploads/wineImages", oldFileName);
            }
            existWine.ModifiedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
        private void CheckEdit(Wine wine, Wine existWine)
        {
            if (existWine.BrandId != wine.BrandId && !_context.Brands.Any(x => x.Id == wine.BrandId))
                ModelState.AddModelError("AuthorId", "Author not found");

            if (existWine.CategoryId != wine.CategoryId && !_context.Categories.Any(x => x.Id == wine.CategoryId))
                ModelState.AddModelError("GenreId", "Genre not found");

            if (existWine.PropertyId != wine.PropertyId && !_context.Properties.Any(x => x.Id == wine.PropertyId))
                ModelState.AddModelError("GenreId", "Genre not found");


            foreach (var colorId in wine.ColorIds)
            {
                if (!_context.Colors.Any(x => x.Id == colorId))
                {
                    ModelState.AddModelError("ColorIds", "ColorIds not found");
                    break;
                }
            }
        }
        public IActionResult Detail(int id)
        {
            Wine wine = _context.Wine.Include(x => x.WineColors).ThenInclude(t => t.color)
                                         .Include(x => x.Brand)
                                         .Include(x => x.Category)
                                         .Include(x => x.Property)
                                         .FirstOrDefault(x => x.Id == id);

            if (wine == null)
                return View("error");

            return View(wine);
        }


        public IActionResult Delete(int id)
        {
            Wine wine = _context.Wine.FirstOrDefault(x => x.Id == id);

            if (wine == null)
                return NotFound();

            wine.IsDeleted = true;
            _context.SaveChanges();
            return Ok();
        }
    }
}
