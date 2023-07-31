using Microsoft.AspNetCore.Mvc;
using WinePage.DAL;
using WinePage.Models;
using WinePage.ViewModels;

namespace WinePage.Controllers
{
    public class HomeController : Controller
    {
        private readonly BodoniDbContext _context;
        public HomeController(BodoniDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            Home_VM home_vm = new Home_VM
            {
                OurClubCards = _context.OurClubCards.ToList(),
                PassionateGrowersCards = _context.PassionateGrowersCards.ToList(),
                Wine = _context.Wine.ToList()
            };
            return View(home_vm);
        }
    }
}