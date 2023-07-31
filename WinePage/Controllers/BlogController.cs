using Microsoft.AspNetCore.Mvc;
using WinePage.DAL;

namespace WinePage.Controllers
{
    public class BlogController : Controller
    {
        private readonly BodoniDbContext _context;

        public BlogController(BodoniDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = _context.Blogs.ToList();
            return View(model);
        }
    }
}
