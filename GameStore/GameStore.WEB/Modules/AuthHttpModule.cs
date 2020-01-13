using GameStore.WEB.Auth.Interfaces;
using System;
using System.Web;
using System.Web.Mvc;

namespace GameStore.WEB.Modules
{
    public class AuthHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(this.Authenticate);
        }

        private void Authenticate(object source, EventArgs e)
        {
            var app = (HttpApplication)source;
            var context = app.Context;

            var auth = DependencyResolver.Current.GetService<IAuthentication>();

            auth.HttpContext = context;
            context.User = auth.CurrentUser;
        }

        public void Dispose()
        {
        }
    }
}