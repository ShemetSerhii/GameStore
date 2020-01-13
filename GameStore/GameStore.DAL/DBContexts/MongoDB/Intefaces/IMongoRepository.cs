using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.DBContexts.MongoDB.Intefaces
{
    public interface IMongoRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get();
        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate,
            Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> sorting = null);
    }
}