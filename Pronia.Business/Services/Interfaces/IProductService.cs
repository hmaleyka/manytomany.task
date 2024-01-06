using Pronia.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pronia.Business.Services.Interfaces
{
    public interface IProductService
    {
        Task<ICollection<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> Create(Product product);
        Task<Product> Update(Product product);

        Task<Product> Delete(Product product);
    }
}
