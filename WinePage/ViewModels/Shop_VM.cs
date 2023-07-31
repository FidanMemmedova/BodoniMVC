using WinePage.Models;

namespace WinePage.ViewModels;

public class Shop_VM
{
    public List<Brand> Brands { get; set; }
    public List<Category> Categories { get; set; }
    public List<Property> Properties { get; set; }
    public List<Color> Colors { get; set; }
    public List<Models.Wine> Wine { get; set; }
    public List<ShopBlog> ShopBlogs { get; set; }
}
