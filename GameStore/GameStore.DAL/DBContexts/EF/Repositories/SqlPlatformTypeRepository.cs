using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GameStore.DAL.DBContexts.EF.Repositories
{
    public class SqlPlatformTypeRepository : IGenericRepository<PlatformType>
    {
        private readonly SqlContext _context;

        public SqlPlatformTypeRepository(SqlContext context)
        {
            _context = context;
        }

        public void Create(PlatformType item)
        {
            _context.PlatformTypes.Add(item);

            _context.SaveChanges();
        }

        public IEnumerable<PlatformType> Get()
        {
            return SeparationOfDeleted();
        }

        public IEnumerable<PlatformType> Get(Func<PlatformType, bool> predicate,
            Func<IEnumerable<PlatformType>, IOrderedEnumerable<PlatformType>> sorting = null)
        {
            var query = SeparationOfDeleted();

            return query.Where(predicate).ToList();
        }

        public void Update(PlatformType item)
        {
            if (_context.Entry(item).State == EntityState.Detached)
            {
                _context.PlatformTypes.Attach(item);
            }

            _context.Entry(item).State = EntityState.Modified;

            _context.SaveChanges();
        }

        public void Remove(PlatformType item)
        {
            item.IsDeleted = true;

            Update(item);
        }

        private IEnumerable<PlatformType> SeparationOfDeleted()
        {
            return _context.PlatformTypes.Where(x => x.IsDeleted == false).ToList();
        }
    }
}
