using System.Collections.Generic;
using GameStore.BLL.FilterPipeline.Abstract;
using GameStore.BLL.FilterPipeline.Concrete;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Identity;
using GameStore.BLL.Services;
using GameStore.BLL.Services.IdentityService;
using GameStore.Domain.Entities;
using Ninject.Modules;

namespace GameStore.Infrastructure.Modules
{
    public class GameModule : NinjectModule
    {
        public override void Load()
        {
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