using GameStore.DAL.Adapters;
using GameStore.DAL.Adapters.Identity;
using GameStore.DAL.DBContexts.EF;
using GameStore.DAL.DBContexts.EF.Repositories;
using GameStore.DAL.DBContexts.EF.Repositories.Identity;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Repositories;
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

            Bind<IGenericRepository<Game>>().To<SqlGameRepository>();
            Bind<IGenericRepository<Comment>>().To<SqlCommentRepository>();
            Bind<IGenericRepository<Genre>>().To<SqlGenreRepository>();
            Bind<IGenericRepository<Order>>().To<SqlOrderRepository>();
            Bind<IGenericRepository<OrderDetail>>().To<SqlOrderDetailRepository>();
            Bind<IGenericRepository<PlatformType>>().To<SqlPlatformTypeRepository>();
            Bind<IGenericRepository<Publisher>>().To<SqlPublisherRepository>();

            Bind<IGenericRepository<User>>().To<UserRepository>();
            Bind<IGenericRepository<Role>>().To<RoleRepository>();

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

            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(_connectionString);
        }
    }
}
