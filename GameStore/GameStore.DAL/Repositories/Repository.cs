using GameStore.DAL.Context;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GameStore.DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly SqlContext _context;
        private readonly DbSet<TEntity> _entitySet;

        public Repository(SqlContext context)
        {
            _context = context;
            _entitySet = _context.Set<TEntity>();
        }

        public void Create(TEntity item)
        {
            _entitySet.Add(item);
        }

        public void Delete(TEntity item)
        {
            _entitySet.Remove(item);
        }

        public async Task<IEnumerable<TEntity>> GetAsync()
        {
            return await _entitySet.ToListAsync();
        }

        public Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> sorting = null)
        {
            return null;
        }

        public Task<TEntity> GetAsync(int id)
        {
            return _entitySet.FindAsync(id);
        }

        public void Update(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}
