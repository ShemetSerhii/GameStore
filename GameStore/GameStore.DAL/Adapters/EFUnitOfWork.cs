using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using GameStore.Domain.Entities.Identity;

namespace GameStore.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        public ICrossAdapter<Game> Games { get; }

        public ICrossAdapter<Comment> Comments { get; }

        public ICrossAdapter<Genre> Genres { get; }

        public ICrossAdapter<Publisher> Publishers { get; }

        public IAdapter<PlatformType> PlatformTypes { get; }

        public IAdapter<OrderDetail> OrderDetails { get; }

        public IAdapter<Order> Orders { get; }

        public IAdapter<User> Users { get; }

        public IAdapter<Role> Roles { get; }

        public IBaseAdapter<Shipper> Shippers { get; }

        public EFUnitOfWork(ICrossAdapter<Game> games, ICrossAdapter<Comment> comments, ICrossAdapter<Genre> genres, ICrossAdapter<Publisher> publishers,
            IAdapter<PlatformType> platformTypes, IAdapter<OrderDetail> orderDetails, IAdapter<Order> orders, IAdapter<User> users, IAdapter<Role> roles, IBaseAdapter<Shipper> shippers)
        {
            Games = games;
            Comments = comments;
            Genres = genres;
            PlatformTypes = platformTypes;
            Publishers = publishers;
            OrderDetails = orderDetails;
            Orders = orders;
            Users = users;
            Roles = roles;
            Shippers = shippers;
        }
    }
}
