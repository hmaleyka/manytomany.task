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
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository _repo;
       
        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }
        public async Task<ICollection<Category>> GetAllAsync()
        {
            var categories = await _repo.GetAllAsync();

            return await categories.ToListAsync();
        }
        public async  Task<Category> Create(Category category)
        {
            if (category == null) throw new Exception("Category should noy be null");
            Category categories = new Category()
            {
                Name = category.Name,
               
            };
            await _repo.Create(category);
            await _repo.SaveChangesAsync();
            return categories;
        }

        public async Task<Category> Delete(Category category)
        {
            //Category categories = await _repo.GetByIdAsync(category.Id);
            if (category == null) throw new Exception("Category should not be null");
            _repo.Delete(category);
            
            await _repo.SaveChangesAsync();
            return category;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            if (id <= 0) throw new Exception("Id can't be less than zero");
            var category = await _repo.GetByIdAsync(id);
            if (category == null) throw new Exception("category should not be null");
            return category;
        }

        public async Task<Category> Update(Category category)
        {
            if (category.Id <= 0) throw new Exception("Id should not be less than zero");
            Category categories = await _repo.GetByIdAsync(category.Id);
            if (category == null) throw new Exception("Category should not be null");
            categories.Name = category.Name;
            
            _repo.Update(category);
            await _repo.SaveChangesAsync();
            return categories;
        }
    }
}
