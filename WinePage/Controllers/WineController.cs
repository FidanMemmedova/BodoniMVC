using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WinePage.DAL;
using WinePage.Models;
using WinePage.ViewModels.Wine;

namespace WinePage.Controllers
{
    public class WineController : Controller
    {
        private readonly BodoniDbContext _context;
        public WineController(BodoniDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail(int id)
        {
            //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Wine wine = _context.Wine
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.Property)
                .Include(x => x.WineColors).ThenInclude(wc => wc.color)
                .FirstOrDefault(x => x.Id == id);


            Wine_VM detailVM = new Wine_VM
            {
                Id = wine.Id,
                Name = wine.Name,
                Brand = wine.Brand.Name,
                Image = wine.Image,
                SalePrice=wine.SalePrice,
                Description=wine.Description,
                ColorName=wine.WineColors.FirstOrDefault(wc=>wc.WineId==id).color.Name
            };

            return View(detailVM);
        }

    }
}
