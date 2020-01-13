using System.Web.Mvc;

namespace GameStore.WEB.Logging
{
    public class LoggerHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            GameStoreLogger.logger.Error(filterContext.Exception, filterContext.Exception.Message, filterContext.Exception.StackTrace);
            base.OnException(filterContext);
        }
    }
}