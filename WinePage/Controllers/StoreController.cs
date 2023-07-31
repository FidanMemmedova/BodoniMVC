using Microsoft.AspNetCore.Mvc;
using WinePage.DAL;

namespace WinePage.Controllers
{
    public class StoreController : Controller
    {
        private readonly BodoniDbContext _context;

        public StoreController(BodoniDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = _context.Stores.ToList();
            return View(model);
        }
    }
}
