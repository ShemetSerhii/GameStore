using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GameStore.DAL.DBContexts.EF.Repositories
{
    public class SqlCommentRepository : IRepository<Comment>
    {
        private readonly SqlContext _context;

        public SqlCommentRepository(SqlContext context)
        {
            _context = context;
        }

        public void Create(Comment comment)
        {
            _context.Comments.Add(comment);

            _context.SaveChanges();
        }

        public IEnumerable<Comment> Get()
        {
            return _context.Comments.ToList();
        }

        public IEnumerable<Comment> Get(Func<Comment, bool> predicate,
            Func<IEnumerable<Comment>, IOrderedEnumerable<Comment>> sorting = null)
        {
            var query = _context.Comments.Where(predicate).ToList();

            return query;
        }

        public void Update(Comment item)
        {
            if (_context.Entry(item).State == EntityState.Detached)
            {
                _context.Comments.Attach(item);
            }

            _context.Entry(item).State = EntityState.Modified;

            _context.SaveChanges();
        }

        public void Remove(Comment item)
        {
            item.IsDeleted = true;

            Update(item);
        }
    }
}
