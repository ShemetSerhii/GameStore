using GameStore.BLL.Interfaces.Identity;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities.Identity;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.BLL.Services.IdentityService
{
    public class RoleService : IRoleService
    {
        private IUnitOfWork unitOfWork { get; }

        public RoleService(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        public Role Get(string name)
        {
            var role = unitOfWork.Roles.Get(x => x.Name == name).SingleOrDefault();

            return role;
        }

        public IEnumerable<Role> GetAll()
        {
            var roles = unitOfWork.Roles.Get();

            return roles;
        }

    }
}
