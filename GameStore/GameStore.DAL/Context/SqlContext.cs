using GameStore.Domain.Entities;
using GameStore.Domain.Entities.Identity;
using System.Data.Entity;

namespace GameStore.DAL.Context
{
    public class SqlContext : DbContext
    {
        public DbSet<Game> Games { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<PlatformType> PlatformTypes { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public SqlContext()
            :base("GameStore")
        {
            Database.SetInitializer(new StoreDbInitializer());
        }
    }
}
