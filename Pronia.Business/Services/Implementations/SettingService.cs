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
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _repo;

        public SettingService(ISettingRepository repo)
        {
            _repo = repo;
        }

        public async Task<Setting> Create(Setting setting)
        {
            if (setting == null) throw new Exception("Key and Value should not be null");
            Setting settings = new Setting()
            {
                Key = setting.Key,
                Value = setting.Value,
            };
            await _repo.Create(setting);
            await _repo.SaveChangesAsync();
            return settings;
        }

        public async Task<Setting> Delete(Setting setting)
        {
            if (setting == null) throw new Exception("Setting should not be null");
            _repo.Delete(setting);
            await _repo.SaveChangesAsync();
            return setting;
        }

        public async Task<ICollection<Setting>> GetAllAsync()
        {
             var settings = await _repo.GetAllAsync();  
            return await settings.ToListAsync();
        }

        public async Task<Setting> GetByIdAsync(int id)
        {
            if (id <= 0) throw new Exception("Id can't be less than zero");
            var setting = await _repo.GetByIdAsync(id);
            if (setting == null) throw new Exception("category should not be null");
            return setting;
        }

        public async Task<Setting> Update(Setting setting)
        {
            if (setting.Id <= 0) throw new Exception("Id should not be less than zero");
            Setting settings = await _repo.GetByIdAsync(setting.Id);
            if (setting == null) throw new Exception("Category should not be null");
            settings.Key = setting.Key;
            settings.Value = setting.Value;


            _repo.Update(setting);
            await _repo.SaveChangesAsync();
            return settings;
        }
    }
}
