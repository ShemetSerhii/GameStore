using GameStore.Domain.Entities.Identity;
using System.Security.Principal;
using System.Web;

namespace GameStore.WEB.Auth.Interfaces
{
    public interface IAuthentication
    {
        HttpContext HttpContext { get; set; }
        User Login(string login);
        User Login(string login, string password, bool isPersistent);
        void LogOut();

        IPrincipal CurrentUser { get; }
    }
}