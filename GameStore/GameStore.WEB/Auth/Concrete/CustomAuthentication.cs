using GameStore.BLL.Interfaces.Identity;
using GameStore.Domain.Entities.Identity;
using GameStore.WEB.Auth.Interfaces;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace GameStore.WEB.Auth.Concrete
{
    public class CustomAuthentication : IAuthentication
    {
        private const string CookieName = "__AUTH_COOKIE";

        private IPrincipal _currentUser;
        private IIdentityService _identityService;

        public HttpContext HttpContext { get; set; }

        public CustomAuthentication(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public User Login(string userName)
        {
            var retUser = _identityService.GetUser(userName);

            if (retUser != null)
            {
                CreateCookie(userName, true);
            }

            return retUser;
        }

        public User Login(string userName, string password, bool isPersistent)
        {
            var retUser = _identityService.Login(userName, password);

            if (retUser != null)
            {
                CreateCookie(userName, isPersistent);
            }

            return retUser;
        }

        public void LogOut()
        {
            var httpCookie = HttpContext.Response.Cookies[CookieName];

            if (httpCookie != null)
            {
                httpCookie.Value = string.Empty;
            }
        }

        public IPrincipal CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    var authCookie = HttpContext.Request.Cookies.Get(CookieName);

                    _currentUser = GetAuthInfo(authCookie);
                }

                return _currentUser;
            }
        }

        private void CreateCookie(string userName, bool isPersistent = true)
        {
            var ticket = new FormsAuthenticationTicket(
                1,
                userName,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                isPersistent,
                string.Empty,
                FormsAuthentication.FormsCookiePath);

            var encTicket = FormsAuthentication.Encrypt(ticket);

            var authCookie = new HttpCookie(CookieName)
            {
                Value = encTicket,
                Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
            };

            HttpContext.Response.Cookies.Set(authCookie);
        }

        private IPrincipal GetAuthInfo(HttpCookie authCookie)
        {
            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
            {
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);

                return new UserProvider(ticket.Name, _identityService);
            }
            else
            {
                return new UserProvider(null, null);
            }
        }
    }
}