using GameStore.DAL.DBContexts.MongoDB.Intefaces;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.Adapters.Identity
{
    public class UserAdapter : IAdapter<User>
    {
        private readonly IGenericRepository<User> _userManager;
        private readonly IMongoRepository<User> _userMongo;

        public UserAdapter(IGenericRepository<User> userManager, IMongoRepository<User> userMongo)
        {
            _userManager = userManager;
            _userMongo = userMongo;
        }

        public IEnumerable<User> Get()
        {
            var users = _userManager.Get().ToList();

            var mongoUsers = _userMongo.Get(x => !users.Select(us => us.Login).Contains(x.Login));

            users.AddRange(mongoUsers);

            return users;
        }

        public IEnumerable<User> Get(Func<User, bool> predicate,
            Func<IEnumerable<User>, IOrderedEnumerable<User>> sorting = null)
        {
            var users = _userManager.Get(predicate, sorting).ToList();

            var userMongo = _userMongo.Get(predicate)
                .Where(x => !users.Select(us => us.Login).Contains(x.Login));

            users.AddRange(userMongo);

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
