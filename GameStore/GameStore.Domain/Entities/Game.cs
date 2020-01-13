using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Entities
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public string Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public short UnitsInStock { get; set; }

        public bool Discontinued { get; set; }

        public int Visits { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DatePublication { get; set; }

        public int? PublisherId { get; set; }

        public Publisher Publisher { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Genre> Genres { get; set; }

        public ICollection<PlatformType> PlatformTypes { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
