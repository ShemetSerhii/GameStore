using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GameStore.DAL.DBContexts.EF.Repositories
{
    public class SqlPublisherRepository : IGenericRepository<Publisher>
    {
        private readonly SqlContext _context;

        public SqlPublisherRepository(SqlContext context)
        {
            _context = context;
        }

        public void Create(Publisher item)
        {
            _context.Publishers.Add(item);

            _context.SaveChanges();
        }

        public IEnumerable<Publisher> Get()
        {
            return _context.Publishers.Where(x => x.IsDeleted == false);
        }

        public IEnumerable<Publisher> Get(Func<Publisher, bool> predicate, 
            Func<IEnumerable<Publisher>, IOrderedEnumerable<Publisher>> sorting = null)
        {
            var publishers = _context.Publishers.Where(predicate);

            publishers = SeparateIsDeleted(publishers);

            return publishers;
        }

        public void Update(Publisher item)
        {
            if (_context.Entry(item).State == EntityState.Detached)
            {
                _context.Publishers.Attach(item);
            }

            _context.Entry(item).State = EntityState.Modified;

            _context.SaveChanges();
        }

        public void Remove(Publisher item)
        {
            item.IsDeleted = true;

            Update(item);
        }

        private IEnumerable<Publisher> SeparateIsDeleted(IEnumerable<Publisher> publishers)
        {
            var filteredPublishers = publishers.Where(x => x.IsDeleted == false);

            foreach (var publisher in filteredPublishers)
            {
               publisher.Games = publisher.Games.Where(x => x.IsDeleted == false).ToList();
            }

            return filteredPublishers;
        }
    }
}
