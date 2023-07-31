using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WinePage.DAL;
using WinePage.Models;
using WinePage.ViewModels;

namespace WinePage.Controllers
{
    public class ShopController : Controller
    {
        private readonly BodoniDbContext _context;

        public ShopController(BodoniDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? brandId = null, List<int> colorIds = null, int? categoryId = null, int? propertyId = null)
        {
            Shop_VM shop_vm = new Shop_VM
            {
                Brands = _context.Brands.ToList(),
                Categories = _context.Categories.ToList(),
                Properties = _context.Properties.ToList(),
                Colors = _context.Colors.ToList(),
                Wine = _context.Wine.ToList(),
                ShopBlogs = _context.ShopBlogs.ToList()
            };




            ViewBag.Brands = _context.Brands.Include(x => x.Wine).ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Properties = _context.Properties.ToList();
            ViewBag.Colors = _context.Colors.ToList();

            ViewBag.BrandId = brandId;
            ViewBag.PropertyId = propertyId;
            ViewBag.CategoryId = categoryId;
            ViewBag.ColorIds = colorIds;



            var query = _context.Wine
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.Property)
                .Include(x => x.WineColors)
                .Where(x => !x.IsDeleted);


            if (brandId != null)
                query = query.Where(x => x.BrandId == brandId);

            if (categoryId != null)
                query = query.Where(x => x.CategoryId == categoryId);

            if (propertyId != null)
                query = query.Where(x => x.PropertyId == propertyId);

            if (colorIds != null && colorIds.Count > 0)
                query = query.Where(x => x.WineColors.Any(x => colorIds.Contains(x.ColorId)));

            return View(shop_vm);
        }
        public List<Wine> Filter(List<int> colorIds, List<int> categories, List<int> properties, List<int> brands)
        {
            List<Wine> wine = _context.Wine.Where(x => categories.Any(c => x.Category.Id == c) && properties.Any(p => x.Property.Id == p) && 
            brands.Any(b => x.Brand.Id == b) && colorIds.Any(c => x.ColorIds.Any(x=> x == c))).ToList();
            return wine;
        }
    }
}

