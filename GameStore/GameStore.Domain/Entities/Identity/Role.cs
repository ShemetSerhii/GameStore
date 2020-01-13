using GameStore.Domain.Entities.Interfaces;
using System.Collections.Generic;

namespace GameStore.Domain.Entities.Identity
{
    public class Role : Entity
    {
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
