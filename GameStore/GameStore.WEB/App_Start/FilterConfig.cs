using System.Web.Mvc;
using GameStore.WEB.Logging;

namespace GameStore.WEB.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            filters.Add(new LoggerHandleErrorAttribute());
            filters.Add(new LogHttpRequest());
        }
    }
}