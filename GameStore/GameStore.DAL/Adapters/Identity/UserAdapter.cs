using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.Adapters.Identity
{
    public class UserAdapter : IAdapter<User>
    {
        private readonly IRepository<User> _userManager;

        public UserAdapter(IRepository<User> userManager)
        {
            _userManager = userManager;
        }

        public IEnumerable<User> Get()
        {
            var users = _userManager.Get().ToList();

            return users;
        }

        public IEnumerable<User> Get(Func<User, bool> predicate,
            Func<IEnumerable<User>, IOrderedEnumerable<User>> sorting = null)
        {
            var users = _userManager.Get(predicate, sorting).ToList();

            return users;
        }

        public void Create(User user)
        {
            if (user.Id != 0)
            {
                Update(user);
            }
            else
            {
                _userManager.Create(user);
            }
        }

        public void Update(User user)
        {
            if (user.Id == 0)
            {
                Create(user);
            }
            else
            {
                _userManager.Update(user);
            }
        }

        public void Remove(User user)
        {
            _userManager.Remove(user);
        }
    }
}
