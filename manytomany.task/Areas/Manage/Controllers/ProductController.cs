
using manytomany.task.Areas.Manage.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace manytomany.task.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        AppDbContext _dbContext;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task <IActionResult> Index()
        {
            List<Product> products =  await _dbContext.products.Include(p=>p.category)
                .Include(p=>p.productTags).ThenInclude(pt=>pt.tag).ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Create ()
        {
            ViewBag.categories= await _dbContext.categories.ToListAsync();
            ViewBag.tags =  await _dbContext.tags.ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM createProductvm)
        {
            ViewBag.categories = await _dbContext.categories.ToListAsync();
            ViewBag.tags = await _dbContext.tags.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            bool resultcategory = await _dbContext.categories.AnyAsync(c => c.Id == createProductvm.CategoryId);
            if(!resultcategory)
            {
                ModelState.AddModelError("CategoryId", "there is not such like that category");
                return View();
            }
            Product product = new Product()
            {
                Name = createProductvm.Name,
                Price = createProductvm.Price,
                Description = createProductvm.Description,
                SKU = createProductvm.SKU,
                CategoryId = createProductvm.CategoryId,
          
            };

            await _dbContext.products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update (int id)
        {
            Product product = await _dbContext.products.Where(p=>p.Id== id).FirstOrDefaultAsync();
            if(product is null)
            {
                return View("Error");
            }
            ViewBag.categories = await _dbContext.categories.ToListAsync();
            ViewBag.tags = await _dbContext.tags.ToListAsync();

            UpdateProductVM updateProductVM = new UpdateProductVM()
            {
                Id = id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SKU = product.SKU,
                CategoryId = product.CategoryId,
            };
            
            return View(updateProductVM);
        }

        [HttpPost] 
        public async Task<IActionResult> Update(UpdateProductVM updateproductvm)
        {

            ViewBag.categories = await _dbContext.categories.ToListAsync();
            ViewBag.tags = await _dbContext.tags.ToListAsync();
            if(!ModelState.IsValid)
            {
                return View();
            }
            Product existproduct = await _dbContext.products.Where(p => p.Id == updateproductvm.Id).FirstOrDefaultAsync();
            if (existproduct is null)
            {
                return View("Error");
            }
            bool resultcategory = await _dbContext.categories.AnyAsync(c => c.Id == updateproductvm.CategoryId);
            if (!resultcategory)
            {
                ModelState.AddModelError("CategoryId", "there is not such like that category");
                return View();
            }
            existproduct.Name = updateproductvm.Name;
            existproduct.Description= updateproductvm.Description;
            existproduct.Price= updateproductvm.Price;
            existproduct.SKU= updateproductvm.SKU;
            existproduct.CategoryId = updateproductvm.CategoryId;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            
            var product = _dbContext.products.FirstOrDefault(p => p.Id == id);
            if(product is null)
            {
                return View("Error");
            }
            _dbContext.products.Remove(product);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
