using GameStore.WEB.Auth.Concrete;
using GameStore.WEB.Auth.Interfaces;
using Ninject.Modules;
using Ninject.Web.Common;

namespace GameStore.WEB.Infrastructure.DependencyInjection
{
    public class WebModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAuthentication>().To<CustomAuthentication>().InRequestScope();
        }
    }
}