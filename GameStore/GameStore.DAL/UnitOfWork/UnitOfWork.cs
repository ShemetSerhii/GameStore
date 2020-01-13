using GameStore.DAL.Context;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Repositories;
using GameStore.Domain.Entities;
using GameStore.Domain.Entities.Identity;
using System;
using System.Threading.Tasks;

namespace GameStore.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlContext _context;
        private readonly Lazy<IRepository<Game>> _gameRepository;
        private readonly Lazy<IRepository<Comment>> _commentRepository;
        private readonly Lazy<IRepository<Genre>> _genreRepository;
        private readonly Lazy<IRepository<Order>> _orderRepository;
        private readonly Lazy<IRepository<OrderDetail>> _orderDetailRepository;
        private readonly Lazy<IRepository<PlatformType>> _platformTypeRepository;
        private readonly Lazy<IRepository<Publisher>> _publisherRepository;
        private readonly Lazy<IRepository<Shipper>> _shipperRepository;
        private readonly Lazy<IRepository<User>> _userRepository;
        private readonly Lazy<IRepository<Role>> _roleRepository;
      
        public UnitOfWork(SqlContext context)
        {
            _context = context;

            _gameRepository = new Lazy<IRepository<Game>>(() => new Repository<Game>(context));
            _commentRepository = new Lazy<IRepository<Comment>>(() => new Repository<Comment>(context));
            _genreRepository = new Lazy<IRepository<Genre>>(() => new Repository<Genre>(context));
            _orderRepository = new Lazy<IRepository<Order>>(() => new Repository<Order>(context));
            _orderDetailRepository = new Lazy<IRepository<OrderDetail>>(() => new Repository<OrderDetail>(context));
            _platformTypeRepository = new Lazy<IRepository<PlatformType>>(() => new Repository<PlatformType>(context));
            _publisherRepository = new Lazy<IRepository<Publisher>>(() => new Repository<Publisher>(context));
            _shipperRepository = new Lazy<IRepository<Shipper>>(() => new Repository<Shipper>(context));
            _userRepository = new Lazy<IRepository<User>>(() => new Repository<User>(context));
            _roleRepository = new Lazy<IRepository<Role>>(() => new Repository<Role>(context));
        }

        public IRepository<Game> GameRepository => _gameRepository.Value;
        
        public IRepository<Comment> CommentRepository => _commentRepository.Value;
        
        public IRepository<Genre> GenreRepository => _genreRepository.Value;
       
        public IRepository<Order> OrderRepository => _orderRepository.Value;
        
        public IRepository<OrderDetail> OrderDetailRepository => _orderDetailRepository.Value;
        
        public IRepository<PlatformType> PlatformTypeRepository => _platformTypeRepository.Value;
        
        public IRepository<Publisher> PublisherRepository => _publisherRepository.Value;
        
        public IRepository<Shipper> ShipperRepository => _shipperRepository.Value;
        
        public IRepository<User> UserRepository => _userRepository.Value;
        
        public IRepository<Role> RoleRepository => _roleRepository.Value;

        public Task SaveAsync()
        {
           return _context.SaveChangesAsync();
        }

    }
}
