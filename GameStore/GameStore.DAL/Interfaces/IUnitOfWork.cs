using GameStore.Domain.Entities;
using GameStore.Domain.Entities.Identity;

namespace GameStore.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Game> Games { get; }

        IRepository<Comment> Comments { get; }

        IRepository<Genre> Genres { get; }

        IRepository<Publisher> Publishers { get; }

        IRepository<PlatformType> PlatformTypes { get; }

        IRepository<OrderDetail> OrderDetails { get; }

        IRepository<Order> Orders { get; }

        IRepository<User> Users { get; }

        IRepository<Role> Roles { get; }

        IRepository<Shipper> Shippers { get; }
    }
}
