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
    public class StoreController : Controller
    {
        private readonly BodoniDbContext _context;
        private readonly IWebHostEnvironment _env;

        public StoreController(BodoniDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public IActionResult Index(int page = 1, int size = 5)
        {
            var query = _context.Stores;

            return View(PaginatedList<Store>.Create(query, page, size));
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Store store)
        {
            if (!ModelState.IsValid)
                return View();

            if (_context.Stores.Any(x => x.Title == store.Title))
            {
                ModelState.AddModelError("Title", "The Store Title is already taken.");
                return View();
            }
            if (_context.Stores.Any(x => x.Subtitle == store.Subtitle))
            {
                ModelState.AddModelError("Subtitle", "The Store Subtitle is already taken.");
                return View();
            }

            if (store.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image is required");
                return View();
            }

            if (store.ImageFile.ContentType != "image/jpeg" && store.ImageFile.ContentType != "image/png")
                ModelState.AddModelError("ImageFile", "ImageFile must be image/png or image/jpeg");

            if (store.ImageFile.Length > 2097152)
                ModelState.AddModelError("ImageFile", "ImageFile must be less or equal than 2MB");

            store.Image = FileManager.Save(store.ImageFile, _env.WebRootPath + "/uploads/storeImages");

            _context.Stores.Add(store);
            _context.SaveChanges();
            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Store store = _context.Stores.Find(id);

            if (store == null)
                return View("Error");

            return View(store);
        }

        [HttpPost]
        public IActionResult Edit(Store store)
        {
            if (!ModelState.IsValid)
                return View();

            Store existStore = _context.Stores.Find(store.Id);

            if (existStore == null)
                return View("Error");

            if (store.Title != existStore.Title && _context.Stores.Any(x => x.Id != store.Id && x.Title == store.Title))
            {
                ModelState.AddModelError("Title", "The Store Title is already taken.");
                return View();
            }
            if (store.Subtitle != existStore.Title && _context.Stores.Any(x => x.Id != store.Id && x.Subtitle == store.Subtitle))
            {
                ModelState.AddModelError("Subtitle", "The Store Subtitle is already taken.");
                return View();
            }
            if (store.ImageFile != null)
            {
                string oldFileName = existStore.Image;
                existStore.Image = FileManager.Save(store.ImageFile, _env.WebRootPath + "/uploads/storeImages");
                FileManager.Delete(_env.WebRootPath + "/uploads/storeImages", oldFileName);
            }

            existStore.Title = store.Title;
            existStore.Subtitle = store.Subtitle;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Store store = _context.Stores.Find(id);

            if (store == null)
                return View("Error");

            return View(store);
        }

        [HttpPost]
        public IActionResult Delete(Store store)
        {
            Store existStore = _context.Stores.Find(store.Id);

            if (existStore == null)
                return View("Error");

            _context.Stores.Remove(existStore);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
