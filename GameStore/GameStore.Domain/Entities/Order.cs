using GameStore.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;

namespace GameStore.Domain.Entities
{
    public class Order : Entity
    {
        public bool IsDeleted { get; set; }

        public string OrderStatus { get; set; }

        public string CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public string Shipper { get; set; }

        public DateTime? ShippedDate { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
