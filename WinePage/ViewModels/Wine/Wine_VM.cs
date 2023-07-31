using WinePage.Models;

namespace WinePage.ViewModels.Wine;

public class Wine_VM
{
    public int Id;
    public string Name { get; set; }
    public string Brand { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
    public decimal SalePrice { get; set; }
    public string ColorName { get; set; }
}
