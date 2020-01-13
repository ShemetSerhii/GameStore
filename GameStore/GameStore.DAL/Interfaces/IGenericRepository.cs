using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Create(TEntity item);
        IEnumerable<TEntity> Get();
        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate, 
            Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> sorting = null);
        void Remove(TEntity item);
        void Update(TEntity item);
    }
}