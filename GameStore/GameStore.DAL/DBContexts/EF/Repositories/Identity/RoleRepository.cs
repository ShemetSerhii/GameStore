using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GameStore.DAL.DBContexts.EF.Repositories.Identity
{
    public class RoleRepository : IGenericRepository<Role>
    {
        private readonly SqlContext _context;

        public RoleRepository(SqlContext context)
        {
            _context = context;
        }

        public void Create(Role item)
        {
            _context.Roles.Add(item);

            _context.SaveChanges();
        }

        public IEnumerable<Role> Get()
        {
            var roles = _context.Roles.ToList();

            return roles;
        }

        public IEnumerable<Role> Get(Func<Role, bool> predicate, Func<IEnumerable<Role>, IOrderedEnumerable<Role>> sorting = null)
        {
            var roles = _context.Roles.Where(predicate).ToList();

            return roles;
        }

        public void Remove(Role item)
        {
            Update(item);
        }

        public void Update(Role item)
        {
            _context.Entry(item).State = EntityState.Modified;

            _context.SaveChanges();
        }
    }
}
