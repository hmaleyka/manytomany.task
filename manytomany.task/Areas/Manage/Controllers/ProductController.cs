using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Pronia.Core.Models;
using Pronia.DAL.Context;
using Pronia.Business.Helpers;
using Pronia.mvc.Areas.Manage.ViewModels.Product;
using Pronia.Business.Services.Interfaces;

namespace Pronia.mvc.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    public class ProductController : Controller
    {
        AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;
        private readonly IProductService _service;

        public ProductController(AppDbContext dbContext, IWebHostEnvironment env , IProductService service)
        {
            _dbContext = dbContext;
            _env = env;
            _service = service;
        }
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Index()
        {
           var products = await _service.GetAllAsync();
            return View(products);
        }
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await _dbContext.categories.ToListAsync();
            ViewBag.tags = await _dbContext.tag.ToListAsync();

            return View();
        }
        [Authorize(Roles = "Admin,Moderator")]
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
                productImages = new List<ProductImage>()
            };
            //await _service.Create(createProductvm);

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

            if (!createProductvm.mainPhoto.CheckType("image/"))
            {
                ModelState.AddModelError("mainPhoto", "you must only apply the image");
                return View();
            }
            if (!createProductvm.mainPhoto.CheckLong(2097152))
            {
                ModelState.AddModelError("mainPhoto", "picture should be less than 3 mb");
                return View();
            }
            if (!createProductvm.hoverPhoto.CheckType("image/"))
            {
                ModelState.AddModelError("hoverPhoto", "you must only apply the image");
                return View();
            }
            if (!createProductvm.hoverPhoto.CheckLong(2097152))
            {
                ModelState.AddModelError("hoverPhoto", "picture should be less than 3 mb");
                return View();
            }

            ProductImage mainphoto = new ProductImage()
            {
                IsPrime = true,
                ImgUrl = createProductvm.mainPhoto.Upload(_env.WebRootPath, @"\Upload\Product\"),
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
                    if (!photo.CheckType("image/"))
                    {
                        TempData["Error"] += $"{photo.FileName} the format isn't correct \t";
                        continue;

                    }
                    if (!photo.CheckLong(2097152))
                    {
                        TempData["Error"] += $"{photo.FileName} the size of picture is not in right format \t";

                        continue;
                    }

                    ProductImage multipleimage = new ProductImage()
                    {
                        IsPrime = null,
                        ImgUrl = photo.Upload(_env.WebRootPath, @"\Upload\Product\"),
                        product = product
                    };
                    product.productImages.Add(multipleimage);
                }



            }

            await _dbContext.products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            Product product = await _dbContext.products.Where(p => p.IsDeleted == false)
                .Include(p => p.category)
                .Include(p => p.productTags)
                .ThenInclude(p => p.tag)
                .Where(p => p.Id == id)
                .Include(p => p.productImages)
                .FirstOrDefaultAsync();
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductVM updateproductvm)
        {

            ViewBag.categories = await _dbContext.categories.ToListAsync();
            ViewBag.tags = await _dbContext.tag.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            Product existproduct = await _dbContext.products.Where(p => p.Id == updateproductvm.Id).Include(p => p.productTags).ThenInclude(p => p.tag).Include(b => b.productImages).FirstOrDefaultAsync();
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




            if (updateproductvm.TagIds != null)
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
                    createTags = updateproductvm.TagIds.ToList();
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
                var productTagList = _dbContext.productTag.Where(pt => pt.ProductId == existproduct.Id).ToList();
                _dbContext.productTag.RemoveRange(productTagList);

            }

            TempData["Error"] = "";

            if (updateproductvm.mainphoto != null)
            {
                if (!updateproductvm.mainphoto.CheckType("image/"))
                {
                    ModelState.AddModelError("mainPhoto", "you must only apply the image");
                    return View();
                }
                if (!updateproductvm.mainphoto.CheckLong(2097152))
                {
                    ModelState.AddModelError("mainPhoto", "picture should be less than 3 mb");
                    return View();
                }
                //ProductImage newproductimage = new ProductImage()
                //{
                //    IsPrime = true,
                //    ProductId = existproduct.Id,
                //    ImgUrl = updateproductvm.mainphoto.Upload(_env.WebRootPath, @"\Upload\Product\")


                //};
                var oldPhoto = existproduct.productImages?.FirstOrDefault(p => p.IsPrime == true);
                existproduct.productImages?.Remove(oldPhoto);
                ProductImage newproductimage = new ProductImage()
                {
                    IsPrime = true,
                    ProductId = existproduct.Id,
                    ImgUrl = updateproductvm.mainphoto.Upload(_env.WebRootPath, @"\Upload\Product\")


                };
                existproduct.productImages?.Add(newproductimage);
            }

            if (updateproductvm.hoverphoto != null)
            {
                if (!updateproductvm.hoverphoto.CheckType("image/"))
                {
                    ModelState.AddModelError("hoverphoto", "you must only apply the image");
                    return View();
                }
                if (!updateproductvm.hoverphoto.CheckLong(2097152))
                {
                    ModelState.AddModelError("hoverphoto", "picture should be less than 3 mb");
                    return View();
                }
                //ProductImage newhover = new ProductImage()
                //{
                //    IsPrime = false,
                //    ProductId = existproduct.Id,
                //    ImgUrl = updateproductvm.hoverphoto.Upload(_env.WebRootPath, @"\Upload\Product\")


                //};
                var oldPhoto = existproduct.productImages?.FirstOrDefault(p => p.IsPrime == false);
                existproduct.productImages?.Remove(oldPhoto);
                ProductImage newhover = new ProductImage()
                {
                    IsPrime = false,
                    ProductId = existproduct.Id,
                    ImgUrl = updateproductvm.hoverphoto.Upload(_env.WebRootPath, @"\Upload\Product\")


                };
                existproduct.productImages?.Add(newhover);
            }
            if (updateproductvm.ImageIds == null)
            {
                existproduct.productImages.RemoveAll(b => b.IsPrime == null);
            }
            else
            {
                var removeListImage = existproduct.productImages?.Where(p => !updateproductvm.ImageIds.Contains(p.Id) && p.IsPrime == null).ToList();
                if (removeListImage != null)
                {
                    foreach (var image in removeListImage)
                    {
                        existproduct.productImages.Remove(image);
                        FileManager.DeleteFile(image.ImgUrl, _env.WebRootPath, @"\Upload\Product\");
                    }


                }
                else
                {
                    existproduct.productImages.RemoveAll(p => p.IsPrime == null);
                }

            }

            if (updateproductvm.multiplephotos != null)
            {
                foreach (var photo in updateproductvm.multiplephotos)
                {
                    if (!photo.CheckType("image/"))
                    {
                        TempData["Error"] += $"{photo.FileName} the format isn't correct \t";
                        continue;

                    }
                    if (!photo.CheckLong(2097152))
                    {
                        TempData["Error"] += $"{photo.FileName} the size of picture is not in right format \t";

                        continue;
                    }

                    ProductImage multipleimage = new ProductImage()
                    {

                        IsPrime = null,
                        ImgUrl = photo.Upload(_env.WebRootPath, @"\Upload\Product\"),
                        product = existproduct
                    };
                    existproduct.productImages?.Add(multipleimage);

                }
            }




            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {

            var product = _dbContext.products.Where(p => p.IsDeleted == false).FirstOrDefault(p => p.Id == id);

            //var relatedProductTag = _dbContext.productstag.               
            //    //.Where(pt => pt.ProductId == id);
            //var relatedProductImage = _dbContext.productsImage.Where(pt => pt.ProductId == id);
            //_dbContext.productTag.RemoveRange(relatedProductTag);
            //_dbContext.productsImage.RemoveRange(relatedProductImage);
            if (product is null)
            {
                return View("Error");
            }

            product.IsDeleted = true;
            _dbContext.SaveChanges();
            return Ok();
            // product.IsDeleted = true;
            //// _dbContext.products.Remove(product);

            // return Ok();
            //return RedirectToAction(nameof(Index));
        }
    }
}
