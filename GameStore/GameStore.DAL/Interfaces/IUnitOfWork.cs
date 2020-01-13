using GameStore.Domain.Entities;
using GameStore.Domain.Entities.Identity;
using System.Threading.Tasks;

namespace GameStore.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Game> GameRepository { get; }

        IRepository<Comment> CommentRepository { get; }

        IRepository<Genre> GenreRepository { get; }

        IRepository<Publisher> PublisherRepository { get; }

        IRepository<PlatformType> PlatformTypeRepository { get; }

        IRepository<OrderDetail> OrderDetailRepository { get; }

        IRepository<Order> OrderRepository { get; }

        IRepository<User> UserRepository { get; }

        IRepository<Role> RoleRepository { get; }

        IRepository<Shipper> ShipperRepository { get; }

        Task SaveAsync();
    }
}
