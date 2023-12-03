using Microsoft.EntityFrameworkCore;

namespace manytomany.task.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {

        AppDbContext _db;

        public ProductViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(int key=1)
        {
            List<Product> products = _db.products.Include(p=>p.productImages).Take(3).ToList();
            switch (key)
            {
                case 1: products = _db.products.Include(p => p.productImages).OrderByDescending(p=>p.Price).Take(3).ToList();
                    break;
                case 2: products = _db.products.Include(p => p.productImages).OrderBy(p=>p.Price).Take(3).ToList();
                    break;
                case 3: products = _db.products.Include(p => p.productImages).OrderByDescending(p=>p.Id).Take(3).ToList();
                    break;
                default: products = _db.products.Include(p => p.productImages).Take(3).ToList();

                    break;
            }



            return View(products);
        }
    }
}
