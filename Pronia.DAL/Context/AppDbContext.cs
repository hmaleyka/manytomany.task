using Pronia.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Pronia.DAL.Context
{
    public class AppDbContext : IdentityDbContext<AppUser>
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
