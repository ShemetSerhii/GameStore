using GameStore.DAL.Context;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Repositories;
using GameStore.DAL.UnitOfWork;
using GameStore.Domain.Entities;
using GameStore.Domain.Entities.Identity;
using Ninject.Modules;

namespace GameStore.Infrastructure.Modules
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
            Bind<SqlContext>().To<SqlContext>();

            Bind<IRepository<Game>>().To<Repository<Game>>();
            Bind<IRepository<Comment>>().To<Repository<Comment>>();
            Bind<IRepository<Genre>>().To<Repository<Genre>>();
            Bind<IRepository<Order>>().To<Repository<Order>>();
            Bind<IRepository<OrderDetail>>().To<Repository<OrderDetail>>();
            Bind<IRepository<PlatformType>>().To<Repository<PlatformType>>();
            Bind<IRepository<Publisher>>().To<Repository<Publisher>>();
            Bind<IRepository<User>>().To<Repository<User>>();
            Bind<IRepository<Role>>().To<Repository<Role>>();

            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(_connectionString);
        }
    }
}
