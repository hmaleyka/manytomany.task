using Pronia.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pronia.Business.Services.Interfaces
{
    public interface ISliderService
    {
        Task<ICollection<Slider>> GetAllAsync();
        Task<Slider> GetByIdAsync(int id);
        Task<Slider> Create(Slider slider);
        Task<Slider> Update(Slider slider);

       Task <Slider> Delete(Slider slider);
    }
}
