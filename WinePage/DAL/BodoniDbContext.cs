using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WinePage.Models;

namespace WinePage.DAL
{
    public class BodoniDbContext : IdentityDbContext
    {
        public BodoniDbContext(DbContextOptions<BodoniDbContext> options) : base(options)
        {

        }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<ShopBlog> ShopBlogs { get; set; }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Wine> Wine { get; set; }
        public DbSet<WineColor> WineColor { get; set; }
        //public DbSet<Order> Orders { get; set; }
        //public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PassionateGrowersCard> PassionateGrowersCards { get; set; }
        public DbSet<OurClubCard> OurClubCards { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }
    }
}
