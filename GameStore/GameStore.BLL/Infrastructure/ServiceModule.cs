using GameStore.DAL.Adapters;
using GameStore.DAL.Adapters.Identity;
using GameStore.DAL.Context;
using GameStore.DAL.DBContexts.EF.Repositories;
using GameStore.DAL.DBContexts.EF.Repositories.Identity;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Repositories;
using GameStore.DAL.UnitOfWork;
using GameStore.Domain.Entities;
using GameStore.Domain.Entities.Identity;
using Ninject.Modules;
using Ninject.Web.Common;

namespace GameStore.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private readonly string _connectionString;

        public ServiceModule(string connection)
        {
            _connectionString = connection;
        }

        public override void Load()
        {
            Bind<SqlContext>().To<SqlContext>().InRequestScope();

            Bind<IRepository<Game>>().To<GameRepository>();
            Bind<IRepository<Comment>>().To<SqlCommentRepository>();
            Bind<IRepository<Genre>>().To<SqlGenreRepository>();
            Bind<IRepository<Order>>().To<SqlOrderRepository>();
            Bind<IRepository<OrderDetail>>().To<SqlOrderDetailRepository>();
            Bind<IRepository<PlatformType>>().To<SqlPlatformTypeRepository>();
            Bind<IRepository<Publisher>>().To<SqlPublisherRepository>();

            Bind<IRepository<User>>().To<UserRepository>();
            Bind<IRepository<Role>>().To<RoleRepository>();

            Bind<ICrossAdapter<Game>>().To<GameAdapter>();
            Bind<ICrossAdapter<Comment>>().To<CommentAdapter>();
            Bind<ICrossAdapter<Genre>>().To<GenreAdapter>();
            Bind<ICrossAdapter<Publisher>>().To<PublisherAdapter>();
            Bind<IAdapter<Order>>().To<OrderAdapter>();
            Bind<IAdapter<OrderDetail>>().To<OrderDetailAdapter>();
            Bind<IAdapter<PlatformType>>().To<PlatformTypeAdapter>();
            Bind<IBaseAdapter<Shipper>>().To<ShipperAdapter>();

            Bind<IAdapter<User>>().To<UserAdapter>();
            Bind<IAdapter<Role>>().To<RoleAdapter>();

            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(_connectionString);
        }
    }
}
