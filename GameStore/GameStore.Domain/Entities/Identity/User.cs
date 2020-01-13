using GameStore.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;

namespace GameStore.Domain.Entities.Identity
{
    public class User : Entity
    {
        public bool IsBanned { get; set; }

        public DateTime? BanExpires { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Login { get; set; }

        public int Password { get; set; }

        public ICollection<Role> Roles { get; set; }
    }
}
