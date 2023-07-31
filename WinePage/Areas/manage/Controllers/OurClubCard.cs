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
    public class OurClubCardController : Controller
    {
        private readonly BodoniDbContext _context;
        private readonly IWebHostEnvironment _env;

        public OurClubCardController(BodoniDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public IActionResult Index(int page = 1, int size = 5)
        {
            var query = _context.OurClubCards;

            return View(PaginatedList<OurClubCard>.Create(query, page, size));
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(OurClubCard OurClubCard)
        {
            if (!ModelState.IsValid)
                return View();

            if (_context.OurClubCards.Any(x => x.Title == OurClubCard.Title))
            {
                ModelState.AddModelError("Title", "The OurClubCard Title is already taken.");
                return View();
            }
            if (_context.OurClubCards.Any(x => x.Description == OurClubCard.Description))
            {
                ModelState.AddModelError("Description", "The OurClubCard Description is already taken.");
                return View();
            }

            if (OurClubCard.IconFile == null)
            {
                ModelState.AddModelError("IconFile", "Icon is required");
                return View();
            }

            if (OurClubCard.IconFile.ContentType != "image/jpeg" && OurClubCard.IconFile.ContentType != "image/png")
                ModelState.AddModelError("IconFile", "Icon must be image/png or image/jpeg");

            if (OurClubCard.IconFile.Length > 2097152)
                ModelState.AddModelError("IconFile", "Icon must be less or equal than 2MB");

            OurClubCard.Icon = FileManager.Save(OurClubCard.IconFile, _env.WebRootPath + "/uploads/OurClubCardImages");

            _context.OurClubCards.Add(OurClubCard);
            _context.SaveChanges();
            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            OurClubCard OurClubCard = _context.OurClubCards.Find(id);

            if (OurClubCard == null)
                return View("Error");

            return View(OurClubCard);
        }

        [HttpPost]
        public IActionResult Edit(OurClubCard OurClubCard)
        {
            if (!ModelState.IsValid)
                return View();

            OurClubCard existOurClubCard = _context.OurClubCards.Find(OurClubCard.Id);

            if (existOurClubCard == null)
                return View("Error");

            if (OurClubCard.Title != existOurClubCard.Title && _context.OurClubCards.Any(x => x.Id != OurClubCard.Id && x.Title == OurClubCard.Title))
            {
                ModelState.AddModelError("Title", "The OurClubCard Title is already taken.");
                return View();
            }
          
            if (OurClubCard.IconFile != null)
            {
                string oldFileName = existOurClubCard.Icon;
                existOurClubCard.Icon = FileManager.Save(OurClubCard.IconFile, _env.WebRootPath + "/uploads/OurClubCardImages");
                FileManager.Delete(_env.WebRootPath + "/uploads/OurClubCardImages", oldFileName);
            }
            existOurClubCard.Title = OurClubCard.Title;
            existOurClubCard.Description = OurClubCard.Description;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            OurClubCard OurClubCard = _context.OurClubCards.Find(id);

            if (OurClubCard == null)
                return View("Error");

            return View(OurClubCard);
        }

        [HttpPost]
        public IActionResult Delete(OurClubCard OurClubCard)
        {
            OurClubCard existOurClubCard = _context.OurClubCards.Find(OurClubCard.Id);

            if (existOurClubCard == null)
                return View("Error");

            _context.OurClubCards.Remove(existOurClubCard);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
