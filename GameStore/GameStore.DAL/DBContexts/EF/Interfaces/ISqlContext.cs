using System.Data.Entity;
using GameStore.Domain.Entities;

namespace GameStore.DAL.EF.Interfaces
{
    public interface ISqlContext
    {
        DbSet<Game> Games { get; set; }

        DbSet<Comment> Comments { get; set; }

        DbSet<Genre> Genres { get; set; }

        DbSet<PlatformType> PlatformTypes { get; set; }

        DbSet<Publisher> Publishers { get; set; }

        DbSet<Order> Orders { get; set; }

        DbSet<OrderDetail> OrderDetails { get; set; }
    }
}