﻿using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Entities
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public string CrossKey { get; set; }

        public int? GameId { get; set; }

        public Game Game { get; set; }

        public decimal Price { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }
    }
}
