using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.Adapters
{
    public class PlatformTypeAdapter : IAdapter<PlatformType>
    {
        private readonly IGenericRepository<PlatformType> _sql;

        public PlatformTypeAdapter(IGenericRepository<PlatformType> platformSql)
        {
            _sql = platformSql;
        }

        public void Create(PlatformType item)
        {
            _sql.Create(item);
        }

        public IEnumerable<PlatformType> Get()
        {
            var platformTypes = _sql.Get();

            return platformTypes;
        }

        public IEnumerable<PlatformType> Get(Func<PlatformType, bool> predicate, 
            Func<IEnumerable<PlatformType>, IOrderedEnumerable<PlatformType>> sorting = null)
        {
            var platformTypes = _sql.Get(predicate, sorting);

            return platformTypes;
        }

        public void Update(PlatformType item)
        {
            _sql.Update(item);
        }

        public void Remove(PlatformType item)
        {
           _sql.Remove(item);
        }
    }
}
