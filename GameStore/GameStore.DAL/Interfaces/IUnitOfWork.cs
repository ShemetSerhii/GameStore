using GameStore.Domain.Entities;
using GameStore.Domain.Entities.Identity;

namespace GameStore.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        ICrossAdapter<Game> Games { get; }
        ICrossAdapter<Comment> Comments { get; }
        ICrossAdapter<Genre> Genres { get; }
        ICrossAdapter<Publisher> Publishers { get; }
        IAdapter<PlatformType> PlatformTypes { get; }
        IAdapter<OrderDetail> OrderDetails { get; }
        IAdapter<Order> Orders { get; }
        IAdapter<User> Users { get; }
        IAdapter<Role> Roles { get; }
        IBaseAdapter<Shipper> Shippers { get; }
    }
}
