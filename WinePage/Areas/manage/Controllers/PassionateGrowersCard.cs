using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WinePage.DAL;
using WinePage.Helpers;
using WinePage.Models;
using WinePage.ViewModels;

namespace WinePage.Areas.manage.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("manage")]
    public class PassionateGrowersCardController : Controller
    {
        private readonly BodoniDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PassionateGrowersCardController(BodoniDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public IActionResult Index(int page = 1, int size = 5)
        {
            var query = _context.PassionateGrowersCards;

            return View(PaginatedList<PassionateGrowersCard>.Create(query, page, size));
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PassionateGrowersCard PassionateGrowersCard)
        {
            if (!ModelState.IsValid)
                return View();

            if (_context.PassionateGrowersCards.Any(x => x.Title == PassionateGrowersCard.Title))
            {
                ModelState.AddModelError("Title", "The PassionateGrowersCard Title is already taken.");
                return View();
            }

            if (PassionateGrowersCard.IconFile == null)
            {
                ModelState.AddModelError("IconFile", "Icon is required");
                return View();
            }

            if (PassionateGrowersCard.IconFile.ContentType != "image/jpeg" && PassionateGrowersCard.IconFile.ContentType != "image/png")
                ModelState.AddModelError("IconFile", "Icon must be image/png or image/jpeg");

            if (PassionateGrowersCard.IconFile.Length > 2097152)
                ModelState.AddModelError("IconFile", "Icon must be less or equal than 2MB");

            PassionateGrowersCard.Icon = FileManager.Save(PassionateGrowersCard.IconFile, _env.WebRootPath + "/uploads/PassionateGrowersCardImages");

            _context.PassionateGrowersCards.Add(PassionateGrowersCard);
            _context.SaveChanges();
            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            PassionateGrowersCard PassionateGrowersCard = _context.PassionateGrowersCards.Find(id);

            if (PassionateGrowersCard == null)
                return View("Error");

            return View(PassionateGrowersCard);
        }

        [HttpPost]
        public IActionResult Edit(PassionateGrowersCard PassionateGrowersCard)
        {
            if (!ModelState.IsValid)
                return View();

            PassionateGrowersCard existPassionateGrowersCard = _context.PassionateGrowersCards.Find(PassionateGrowersCard.Id);

            if (existPassionateGrowersCard == null)
                return View("Error");

            if (PassionateGrowersCard.Title != existPassionateGrowersCard.Title && _context.PassionateGrowersCards.Any(x => x.Id != PassionateGrowersCard.Id && x.Title == PassionateGrowersCard.Title))
            {
                ModelState.AddModelError("Title", "The PassionateGrowersCard Title is already taken.");
                return View();
            }
            if (PassionateGrowersCard.Description != existPassionateGrowersCard.Title && _context.PassionateGrowersCards.Any(x => x.Id != PassionateGrowersCard.Id && x.Description == PassionateGrowersCard.Description))
            {
                ModelState.AddModelError("Description", "The PassionateGrowersCard Description is already taken.");
                return View();
            }
            if (PassionateGrowersCard.IconFile != null)
            {
                string oldFileName = existPassionateGrowersCard.Icon;
                existPassionateGrowersCard.Icon = FileManager.Save(PassionateGrowersCard.IconFile, _env.WebRootPath + "/uploads/PassionateGrowersCardImages");
                FileManager.Delete(_env.WebRootPath + "/uploads/PassionateGrowersCardImages", oldFileName);
            }
            existPassionateGrowersCard.Title = PassionateGrowersCard.Title;
            existPassionateGrowersCard.Description = PassionateGrowersCard.Description;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            PassionateGrowersCard PassionateGrowersCard = _context.PassionateGrowersCards.Find(id);

            if (PassionateGrowersCard == null)
                return View("Error");

            return View(PassionateGrowersCard);
        }

        [HttpPost]
        public IActionResult Delete(PassionateGrowersCard PassionateGrowersCard)
        {
            PassionateGrowersCard existPassionateGrowersCard = _context.PassionateGrowersCards.Find(PassionateGrowersCard.Id);

            if (existPassionateGrowersCard == null)
                return View("Error");

            _context.PassionateGrowersCards.Remove(existPassionateGrowersCard);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
