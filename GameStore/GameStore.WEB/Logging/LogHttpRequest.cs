using System.Web.Mvc;

namespace GameStore.WEB.Logging
{
    public class LogHttpRequest : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            GameStoreLogger.logger.Debug($"Request finished");
            base.OnActionExecuted(filterContext);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            GameStoreLogger.logger.Debug($"Request started: {filterContext.HttpContext.Request.Url}");
            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            GameStoreLogger.logger.Debug("Time of current HTTP Request: " + filterContext.HttpContext.Timestamp + '\n' + "-----------------");
            base.OnResultExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            GameStoreLogger.logger.Debug("Current User: " + filterContext.HttpContext.User.Identity.Name);
            base.OnResultExecuting(filterContext);
        }
    }
}