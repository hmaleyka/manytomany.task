
using manytomany.task.Areas.Manage.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
namespace manytomany.task.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _dbContext.products.Include(p => p.category)
                .Include(p => p.productTags)
                .ThenInclude(pt => pt.tag)
                .Include(p=>p.productImages).ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await _dbContext.categories.ToListAsync();
            ViewBag.tags = await _dbContext.tag.ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM createProductvm)
        {
            ViewBag.categories = await _dbContext.categories.ToListAsync();
            ViewBag.tags = await _dbContext.tag.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            bool resultcategory = await _dbContext.categories.AnyAsync(c => c.Id == createProductvm.CategoryId);
            if (!resultcategory)
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
                productImages= new List<ProductImage>()
            };

            if (createProductvm.TagIds != null)
            {
                foreach (var tagId in createProductvm.TagIds)
                {
                    bool resultTag = await _dbContext.tag.AnyAsync(c => c.Id == tagId);
                    if (!resultTag)
                    {
                        ModelState.AddModelError("TagIds", "there is not such like tag here");
                        return View();
                    }


                    ProductTag productTag = new ProductTag()
                    {
                        product = product,
                        TagId = tagId
                    };
                    _dbContext.productTag.AddAsync(productTag);

                }
            }

            if(!createProductvm.mainPhoto.ContentType.StartsWith("image/"))
            {
                ModelState.AddModelError("mainPhoto", "you must only apply the image");
                return View();
            }
            if (createProductvm.mainPhoto.Length > 2097152)
            {
                ModelState.AddModelError("mainPhoto", "picture should be less than 3 mb");
                return View();
            }
            if (!createProductvm.hoverPhoto.ContentType.StartsWith("image/"))
            {
                ModelState.AddModelError("hoverPhoto", "you must only apply the image");
                return View();
            }
            if (createProductvm.hoverPhoto.Length > 2097152)
            {
                ModelState.AddModelError("hoverPhoto", "picture should be less than 3 mb");
                return View();
            }

            ProductImage mainphoto = new ProductImage()
            {
                IsPrime = true,
                ImgUrl=createProductvm.mainPhoto.Upload(_env.WebRootPath , @"\Upload\Product\"),
                product = product
            };

            ProductImage hoverphoto = new ProductImage()
            {
                IsPrime = false,
                ImgUrl = createProductvm.hoverPhoto.Upload(_env.WebRootPath, @"\Upload\Product\"),
                product = product
            };
            TempData["Error"] = "";

            product.productImages.Add(mainphoto);
            product.productImages.Add(hoverphoto);

            if (createProductvm.multipleImages != null)
            {
                foreach (var photo in createProductvm.multipleImages)
                {
                    if (!photo.ContentType.StartsWith("image/"))
                    {
                        TempData["Error"] += $"{photo.FileName} the format isn't correct \t";
                        continue;
                        
                    }
                    if (photo.Length > 2097152)
                    {
                        TempData["Error"] += $"{photo.FileName} the size of picture is not in right format \t";
                        
                        continue;
                    }

                    ProductImage multipleimage = new ProductImage()
                    {
                        IsPrime = null,
                        ImgUrl = createProductvm.hoverPhoto.Upload(_env.WebRootPath, @"\Upload\Product\"),
                        product = product
                    };
                    product.productImages.Add(multipleimage);
                }



            }

            await _dbContext.products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            Product product = await _dbContext.products
                .Include(p => p.category)
                .Include(p => p.productTags)
                .ThenInclude(p => p.tag)
                .Include(p=>p.productImages)
                .Where(p => p.Id == id).FirstOrDefaultAsync();
            if (product is null)
            {
                return View("Error");
            }
            ViewBag.categories = await _dbContext.categories.ToListAsync();
            ViewBag.tags = await _dbContext.tag.ToListAsync();

            UpdateProductVM updateProductVM = new UpdateProductVM()
            {
                Id = id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SKU = product.SKU,
                CategoryId = product.CategoryId,
                TagIds = new List<int>(),    //tagId 0 dan baslatdq viewdn dussun deye taglar
                allproductImages = new List<ProductImagesVm>()
            };

            foreach (var item in product.productTags)
            {
                updateProductVM.TagIds.Add((int)item.TagId);
            }

            foreach (var item in product.productImages)
            {
                ProductImagesVm productImages = new ProductImagesVm()
                {
                    IsPrime = item.IsPrime,
                    ImgUrl = item.ImgUrl,
                    Id = item.Id,
                };

                updateProductVM.allproductImages.Add(productImages);
            }

            return View(updateProductVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductVM updateproductvm)
        {

            ViewBag.categories = await _dbContext.categories.ToListAsync();
            ViewBag.tags = await _dbContext.tag.ToListAsync();
            if (!ModelState.IsValid)
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
            existproduct.Description = updateproductvm.Description;
            existproduct.Price = updateproductvm.Price;
            existproduct.SKU = updateproductvm.SKU;
            existproduct.CategoryId = updateproductvm.CategoryId;


            if (updateproductvm != null)
            {
                foreach (var tagId in updateproductvm.TagIds)
                {
                    bool resultTag = await _dbContext.tag.AnyAsync(c => c.Id == tagId);
                    if (!resultTag)
                    {
                        ModelState.AddModelError("TagIds", "there is not such like tag here");
                        return View();
                    }


                }
                //yeni yaranmis tag gelir vm deki id yoxlayir eyni olan

                List<int> createTags;
                if (existproduct.productTags != null)
                {
                    createTags = updateproductvm.TagIds.Where(ti => !existproduct.productTags.Exists(pt => pt.TagId == ti)).ToList();

                }
                else
                {
                    createTags=updateproductvm.TagIds.ToList();
                }

                foreach (var tagid in createTags)
                {
                    ProductTag productTag = new ProductTag()
                    {
                        TagId = tagid,
                        ProductId = existproduct.Id
                    };
                    //existproduct.productTags.Add(productTag);

                    await _dbContext.productTag.AddAsync(productTag);

                }

                List<ProductTag> removeTag = existproduct.productTags.Where(pt => !updateproductvm.TagIds.Contains((int)pt.TagId)).ToList();

                _dbContext.productTag.RemoveRange(removeTag);

            }
            else
            {
                var productTagList= _dbContext.productTag.Where(pt=>pt.ProductId == existproduct.Id).ToList();
                _dbContext.productTag.RemoveRange(productTagList);
            
            }



            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {

            var product = _dbContext.products.FirstOrDefault(p => p.Id == id);
            if (product is null)
            {
                return View("Error");
            }
            _dbContext.products.Remove(product);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
