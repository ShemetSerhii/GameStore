using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GameStore.DAL.DBContexts.EF.Repositories
{
    public class SqlGenreRepository : IRepository<Genre>
    {
        private readonly SqlContext _context;

        public SqlGenreRepository(SqlContext context)
        {
            _context = context;
        }

        public void Create(Genre item)
        {

            _context.Genres.Add(item);

            _context.SaveChanges();
        }

        public IEnumerable<Genre> Get()
        {
            return VerificationIsDeleted();
        }

        public IEnumerable<Genre> Get(Func<Genre, bool> predicate, 
            Func<IEnumerable<Genre>, IOrderedEnumerable<Genre>> sorting = null)
        {
            var query = VerificationIsDeleted().Where(predicate);

            return query.Where(predicate).ToList();
        }

        public void Update(Genre item)
        {
            if (_context.Entry(item).State == EntityState.Detached)
            {
                _context.Genres.Attach(item);
            }

            _context.Entry(item).State = EntityState.Modified;

            _context.SaveChanges();
        }

        public void Remove(Genre item)
        {
            item.IsDeleted = true;

            Update(item);
        }

        private IEnumerable<Genre> VerificationIsDeleted()
        {
            var genres = _context.Genres.Where(x => x.IsDeleted == false).ToList();

            return genres;
        }
    }
}
