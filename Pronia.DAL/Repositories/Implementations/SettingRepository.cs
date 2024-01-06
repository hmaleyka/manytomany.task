using Pronia.Core.Models;
using Pronia.DAL.Context;
using Pronia.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pronia.DAL.Repositories.Implementations
{
    public class SettingRepository : Repository<Setting>, ISettingRepository
    {
        public SettingRepository(AppDbContext dbcontext) : base(dbcontext)
        {
        }
    }
}
