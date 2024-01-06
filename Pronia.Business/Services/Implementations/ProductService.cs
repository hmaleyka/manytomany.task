using Microsoft.EntityFrameworkCore;
using Pronia.Business.Services.Interfaces;
using Pronia.Core.Models;
using Pronia.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pronia.Business.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<Product> Create(Product product)
        {

            Product products = new Product()
            {
                Name = product.Name,
                Description = product.Description,
                SKU = product.SKU,
                Price = product.Price,
                CategoryId = product.CategoryId,
               // productImages = product.productImages,
                //productTags = product.productTags
                productImages = new List<ProductImage>()
            };
            //ProductImage mainphoto = new ProductImage()
            //{
            //    IsPrime = true,
            //    ImgUrl = product.mainPhoto.Upload(_env.WebRootPath, @"\Upload\Product\"),
            //    product = product
            //};
            await _repo.Create(products);
            await _repo.SaveChangesAsync();
            return products;

        }

        public Task<Product> Delete(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Product>> GetAllAsync()
        {
            var products = await _repo.GetAllAsync();
            return await products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var products = await _repo.GetByIdAsync(id);
            return products;
        }

        public async Task<Product> Update(Product product)
        {
            Product products = await _repo.GetByIdAsync(product.Id);
            products.Name = product.Name;
            products.Description = product.Description;
            products.SKU = product.SKU;
            products.Price = product.Price;
            products.CategoryId = product.CategoryId;
            products.productImages = product.productImages;
            products.productTags = product.productTags;
            _repo.Update(product);
            await _repo.SaveChangesAsync();
            return products;
        }
    }
}
