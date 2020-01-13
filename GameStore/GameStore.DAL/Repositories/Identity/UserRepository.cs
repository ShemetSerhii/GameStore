using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GameStore.DAL.DBContexts.EF.Repositories.Identity
{
    public class UserRepository : IRepository<User>
    {
        private readonly SqlContext _context;

        public UserRepository(SqlContext context)
        {
            _context = context;
        }

        public IEnumerable<User> Get()
        {
            var users = _context.Users.ToList();

            return users;
        }

        public IEnumerable<User> Get(Func<User, bool> predicate,
            Func<IEnumerable<User>, IOrderedEnumerable<User>> sorting = null)
        {
            var users = _context.Users.Where(predicate).ToList();

            return users;
        }

        public void Create(User user)
        {
            _context.Users.Add(user);

            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;

            _context.SaveChanges();
        }

        public void Remove(User user)
        {
             Update(user);
        }
    }
}
