using Pronia.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pronia.Business.Services.Interfaces
{
    public  interface ISettingService
    {
        Task<ICollection<Setting>> GetAllAsync();
        Task<Setting> GetByIdAsync(int id);
        Task<Setting> Create(Setting setting);
        Task<Setting> Update(Setting setting);

        Task<Setting> Delete(Setting setting);
    }
}
