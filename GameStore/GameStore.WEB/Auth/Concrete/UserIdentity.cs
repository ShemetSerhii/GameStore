using GameStore.BLL.Interfaces.Identity;
using GameStore.Domain.Entities.Identity;
using GameStore.WEB.Auth.Interfaces;
using System.Security.Principal;

namespace GameStore.WEB.Auth.Concrete
{
    public class UserIdentity : IIdentity, IUserProvider
    {
        public User User { get; set; }

        public string AuthenticationType
        {
            get
            {
                return typeof(User).ToString();
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return User != null;
            }
        }

        public string Name
        {
            get
            {
                if (User != null)
                {
                    return User.Login;
                }

                return "anonym";
            }
        }

        public void Init(string login, IIdentityService service)
        {
            if (!string.IsNullOrEmpty(login))
            {
                User = service.GetUser(login);
            }
        }
    }
}