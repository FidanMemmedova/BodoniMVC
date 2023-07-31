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
    public class BlogController : Controller
    {
        private readonly BodoniDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogController(BodoniDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public IActionResult Index(int page = 1, int size = 5)
        {
            var query = _context.Blogs;

            return View(PaginatedList<Blog>.Create(query, page, size));
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Blog Blog, IFormFile file)
        {
            if (!ModelState.IsValid)
                return View();

            if (_context.Blogs.Any(x => x.Title == Blog.Title))
            {
                ModelState.AddModelError("Title", "The Blog Title is already taken.");
                return View();
            }
            if (Blog.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image is required");
                return View();
            }

            if (Blog.ImageFile.ContentType != "image/jpeg" && Blog.ImageFile.ContentType != "image/png")
                ModelState.AddModelError("ImageFile", "ImageFile must be image/png or image/jpeg");

            if (Blog.ImageFile.Length > 2097152)
                ModelState.AddModelError("ImageFile", "ImageFile must be less or equal than 2MB");

            Blog.Image = FileManager.Save(Blog.ImageFile, _env.WebRootPath + "/uploads/blogImages");

            _context.Blogs.Add(Blog);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Blog Blog = _context.Blogs.Find(id);

            if (Blog == null)
                return View("Error");

            return View(Blog);
        }

        [HttpPost]
        public IActionResult Edit(Blog Blog)
        {
            if (!ModelState.IsValid)
                return View();

            Blog existBlog = _context.Blogs.Find(Blog.Id);

            if (existBlog == null)
                return View("Error");

            if (Blog.Title != existBlog.Title && _context.Blogs.Any(x => x.Id != Blog.Id && x.Title == Blog.Title))
            {
                ModelState.AddModelError("Name", "The Blog name is already taken.");
                return View();
            }
            if (Blog.Desc != Blog.Title && _context.Blogs.Any(x => x.Id != Blog.Id && x.Desc == Blog.Desc))
            {
                ModelState.AddModelError("Desc", "The Blog Description is already taken.");
                return View();
            }
            if (Blog.ImageFile != null)
            {
                string oldFileName = existBlog.Image;
                existBlog.Image = FileManager.Save(Blog.ImageFile, _env.WebRootPath + "/uploads/blogImages");
                FileManager.Delete(_env.WebRootPath + "/uploads/blogImages", oldFileName);
            }

            existBlog.Title = Blog.Title;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Blog Blog = _context.Blogs.Find(id);

            if (Blog == null)
                return View("Error");

            return View(Blog);
        }

        [HttpPost]
        public IActionResult Delete(Blog Blog)
        {
            Blog existBlog = _context.Blogs.Find(Blog.Id);

            if (existBlog == null)
                return View("Error");

            _context.Blogs.Remove(existBlog);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
