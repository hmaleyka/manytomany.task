using manytomany.task.Models;
using Microsoft.EntityFrameworkCore;

namespace manytomany.task.DAL
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        
        
        }

        public DbSet<Product> products { get; set; }
        public DbSet<ProductImage> productsImage { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<ProductTag> productTag { get; set; }
        public DbSet<Tag> tag { get; set; }
        public DbSet<Slider> sliders { get; set; }
        public DbSet<Setting> setting { get; set; }
    }
}
