using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.Adapters.Identity
{
    public class RoleAdapter : IAdapter<Role>
    {
        private readonly IGenericRepository<Role> _roleManager;

        public RoleAdapter(IGenericRepository<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public IEnumerable<Role> Get()
        {
            var roles = _roleManager.Get();

            return roles;
        }

        public IEnumerable<Role> Get(Func<Role, bool> predicate,
            Func<IEnumerable<Role>, IOrderedEnumerable<Role>> sorting = null)
        {
            var roles = _roleManager.Get(predicate, sorting);

            return roles;
        }

        public void Create(Role role)
        {
            _roleManager.Create(role);
        }

        public void Update(Role role)
        {
            _roleManager.Update(role);
        }

        public void Remove(Role role)
        {
            _roleManager.Remove(role);
        }
    }
}
