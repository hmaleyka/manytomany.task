using Pronia.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pronia.Business.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ICollection<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<Category> Create(Category category);
        Task<Category> Update(Category category);

        Task<Category> Delete(Category category);
    }
}
