using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WinePage.DAL;
using WinePage.Helpers;
using WinePage.Models;
using WinePage.ViewModels;
using static System.Formats.Asn1.AsnWriter;

namespace WinePage.Areas.manage.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Area("manage")]
    public class ShopBlogController : Controller
    {
        private readonly BodoniDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ShopBlogController(BodoniDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public IActionResult Index(int page = 1, int size = 5)
        {
            var query = _context.ShopBlogs;

            return View(PaginatedList<ShopBlog>.Create(query, page, size));
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ShopBlog ShopBlog, IFormFile file)
        {
            if (!ModelState.IsValid)
                return View();

            if (_context.ShopBlogs.Any(x => x.Title == ShopBlog.Title))
            {
                ModelState.AddModelError("Title", "The ShopBlog Title is already taken.");
                return View();
            }
            if (ShopBlog.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image is required");
                return View();
            }

            if (ShopBlog.ImageFile.ContentType != "image/jpeg" && ShopBlog.ImageFile.ContentType != "image/png")
                ModelState.AddModelError("ImageFile", "ImageFile must be image/png or image/jpeg");

            if (ShopBlog.ImageFile.Length > 2097152)
                ModelState.AddModelError("ImageFile", "ImageFile must be less or equal than 2MB");

            ShopBlog.Image = FileManager.Save(ShopBlog.ImageFile, _env.WebRootPath + "/uploads/ShopBlogImages");

            _context.ShopBlogs.Add(ShopBlog);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            ShopBlog ShopBlog = _context.ShopBlogs.Find(id);

            if (ShopBlog == null)
                return View("Error");

            return View(ShopBlog);
        }

        [HttpPost]
        public IActionResult Edit(ShopBlog ShopBlog)
        {
            if (!ModelState.IsValid)
                return View();

            ShopBlog existShopBlog = _context.ShopBlogs.Find(ShopBlog.Id);

            if (existShopBlog == null)
                return View("Error");

            if (ShopBlog.Title != existShopBlog.Title && _context.ShopBlogs.Any(x => x.Id != ShopBlog.Id && x.Title == ShopBlog.Title))
            {
                ModelState.AddModelError("Name", "The ShopBlog name is already taken.");
                return View();
            }
            if (ShopBlog.Desc != ShopBlog.Title && _context.ShopBlogs.Any(x => x.Id != ShopBlog.Id && x.Desc == ShopBlog.Desc))
            {
                ModelState.AddModelError("Desc", "The ShopBlog Description is already taken.");
                return View();
            }
            if (ShopBlog.ImageFile != null)
            {
                string oldFileName = existShopBlog.Image;
                existShopBlog.Image = FileManager.Save(ShopBlog.ImageFile, _env.WebRootPath + "/uploads/ShopBlogImages");
                FileManager.Delete(_env.WebRootPath + "/uploads/ShopBlogImages", oldFileName);
            }

            existShopBlog.Title = ShopBlog.Title;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            ShopBlog ShopBlog = _context.ShopBlogs.Find(id);

            if (ShopBlog == null)
                return View("Error");

            return View(ShopBlog);
        }

        [HttpPost]
        public IActionResult Delete(ShopBlog ShopBlog)
        {
            ShopBlog existShopBlog = _context.ShopBlogs.Find(ShopBlog.Id);

            if (existShopBlog == null)
                return View("Error");

            _context.ShopBlogs.Remove(existShopBlog);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
