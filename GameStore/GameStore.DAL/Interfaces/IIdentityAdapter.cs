using GameStore.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.Interfaces
{
    public interface IUserAdapter
    {
        IEnumerable<User> Get();
        IEnumerable<User> Get(Func<User, bool> predicate,
            Func<IEnumerable<User>, IOrderedEnumerable<User>> sorting = null);
        void Create(User user);
        void Update(User user);
        void Remove(User user);
    }
}