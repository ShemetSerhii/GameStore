using GameStore.BLL.FilterPipeline.Abstract;
using GameStore.BLL.FilterPipeline.Concrete;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Identity;
using GameStore.BLL.Services;
using GameStore.BLL.Services.IdentityService;
using GameStore.Domain.Entities;
using GameStore.WEB.Auth.Concrete;
using GameStore.WEB.Auth.Interfaces;
using Ninject.Modules;
using Ninject.Web.Common;
using System.Collections.Generic;

namespace GameStore.WEB.Util
{
    public class GameModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAuthentication>().To<CustomAuthentication>().InRequestScope();

            Bind<IIdentityService>().To<IdentityService>();
            Bind<IRoleService>().To<RoleService>();

            Bind<IGameService>().To<GameService>();
            Bind<IGenreService>().To<GenreService>();
            Bind<ICommentService>().To<CommentService>();
            Bind<IPublisherService>().To<PublisherService>();
            Bind<IOrderService>().To<OrderService>();
            Bind<IShipperService>().To<ShipperService>();

            Bind<Pipeline<IEnumerable<Game>>>().To<GameSelectionPipeline>();
        }
    }
}