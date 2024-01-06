using Microsoft.EntityFrameworkCore;
using Pronia.Core.Models.Entity;
using Pronia.DAL.Context;
using Pronia.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pronia.DAL.Repositories.Implementations
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly AppDbContext _dbcontext;
        private DbSet<TEntity> _table;

        public Repository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
            _table = _dbcontext.Set<TEntity>();
        }
        public DbSet<TEntity> Table => _dbcontext.Set<TEntity>();

        public async Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? expression = null,
           Expression<Func<TEntity, object>>? OrderByExpression = null,
           bool isDescending = false
           , params string[] includes)
        {
            IQueryable<TEntity> query = Table;
            if (expression is not null)
            {
                query = query.Where(expression);
            }
            if (includes is not null)
            {
                for (int i = 0; i < includes.Length; i++)
                {
                    query = query.Include(includes[i]);
                }
            }

            if (OrderByExpression != null)
            {
                query = isDescending ? query.OrderByDescending(OrderByExpression) : query.OrderBy(OrderByExpression);
            }


            return query;
        }
        public async Task Create(TEntity entity)
        {
            await _table.AddAsync(entity);
        }

        public  void Delete(TEntity entity)
        {
            _table.Remove(entity);
            
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            TEntity entity = await _table.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            return entity;
        }

        public async Task SaveChangesAsync()
        {
            await _dbcontext.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            _table.Update(entity);
        }
    }
}
