using System.Linq;
using System.Security.Principal;
using GameStore.BLL.Interfaces.Identity;

namespace GameStore.WEB.Auth.Concrete
{
    public class UserProvider : IPrincipal
    {
        private UserIdentity userIdentity { get; set; }

        public IIdentity Identity
        {
            get
            {
                return userIdentity;
            }
        }

        public bool IsInRole(string role)
        {
            if (userIdentity.User == null)
            {
                if (role == "Guest")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return userIdentity.User.Roles.Select(rol => rol.Name).Contains(role);
        }

        public UserProvider(string name, IIdentityService service)
        {
            userIdentity = new UserIdentity();
            userIdentity.Init(name, service);
        }


        public override string ToString()
        {
            return userIdentity.Name;
        }
    }
}