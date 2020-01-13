using System.Web.Mvc;
using System.Web.Routing;

namespace GameStore.WEB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "GetAllGame",
                url: "games",
                defaults: new { controller = "Game", action = "Index" }
            );

            routes.MapRoute(
                name: "ViewBasket",
                url: "basket",
                defaults: new { controller = "Order", action = "GetBasket" }
            );

            routes.MapRoute(
                name: "MakeOrder",
                url: "order",
                defaults: new { controller = "Order", action = "MakeOrder" }
            );

            routes.MapRoute(
              name: "Orders",
              url: "orders",
              defaults: new { controller = "Orders", action = "Index" }
          );

            routes.MapRoute(
                name: "Controller",
                url: "{controller}",
                defaults: new { action = "Index" }
            );

            routes.MapRoute(
                name: "GameDetails",
                url: "game/{key}",
                defaults: new { controller = "Game", action = "Details" }
            );

            routes.MapRoute(
                name: "Buy",
                url: "game/{key}/buy",
                defaults: new { controller = "Order", action = "AddIntoBasket" }
            );

            routes.MapRoute(
                name: "Game",
                url: "game/{key}/{action}",
                defaults: new { controller = "Game" }
            );

            routes.MapRoute(
                name: "DefaultGames",
                url: "games/{action}",
                defaults: new { controller = "Game" }
            );

            routes.MapRoute(
                name: "PublisherCreate",
                url: "publisher/new",
                defaults: new { controller = "Publisher", action = "Create" }
            );

            routes.MapRoute(
                name: "PublisherDetails",
                url: "publisher/{CompanyName}",
                defaults: new { controller = "Publisher", action = "Details" }
            );

            routes.MapRoute(
                name: "PublisherDetails2",
                url: "publisher/{CompanyName}/{action}",
                defaults: new { controller = "Publisher"}
            );

            routes.MapRoute(
                name: "AdminUser",
                url: "admin/{login}",
                defaults: new { controller = "Admin", action = "UserDetails" }
            );

            routes.MapRoute(
                name: "AdminUserAction",
                url: "admin/{login}/{action}",
                defaults: new { controller = "Admin" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional }
            );          
        }
    }
}
