using GameStore.Domain.Entities.Identity;
using System.Collections.Generic;

namespace GameStore.BLL.Interfaces.Identity
{
    public interface IRoleService
    {
        Role Get(string name);
        IEnumerable<Role> GetAll();
    }
}