using GameStore.WEB.App_Start;
using GameStore.WEB.AutoMapper;
using GameStore.WEB.Infrastructure.DependencyInjection;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GameStore.WEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            AutoMapperConfiguration configuration = new AutoMapperConfiguration();
            configuration.Config();

            DependencyConfiguration.Configure();

        }

        protected void Application_BeginRequest()
        {
            string cultureName;

            var cultureCookie = HttpContext.Current.Request.Cookies["lang"];

            if (cultureCookie != null)
            {
                cultureName = cultureCookie.Value;
            }
            else
            {
                cultureName = "en";
            }
            
            var cultures = new List<string>() { "ru", "en"};

            if (!cultures.Contains(cultureName))
            {
                cultureName = "en";
            }

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
        }
    }
}
