using System.Web.Mvc;
using GameStore.Infrastructure.Modules;
using Ninject;
using Ninject.Web.Mvc;

namespace GameStore.WEB.Infrastructure.DependencyInjection
{
    public static class DependencyConfiguration
    {
        public static void Configure()
        {
            var gameModule = new GameModule();
            var serviceModule = new ServiceModule("Connection");
            var webModule = new WebModule();
           
            var kernel = new StandardKernel(gameModule, serviceModule, webModule);
           
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}