using System;
using System.Collections.Generic;

namespace GameStore.Domain.Entities.Identity
{
    public class User
    {
        public int Id { get; set; }
        public bool IsBanned { get; set; }
        public DateTime? BanExpires { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Login { get; set; }
        public int Password { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public User()
        {
            Roles = new List<Role>();
        }
    }
}
