using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.Interfaces
{
    public interface IBaseAdapter<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get();
        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate,
            Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> sorting = null);
    }
}