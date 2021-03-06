﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GameStore.DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Create(TEntity item);
        
        Task<IEnumerable<TEntity>> GetAsync();

        Task<TEntity> GetAsync(int id);

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, 
            Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> sorting = null);
        
        void Delete(TEntity item);
        
        void Update(TEntity item);
    }
}