﻿using System;
using System.Collections.Generic;

namespace GameStore.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public string CrossId { get; set; }

        public bool IsDeleted { get; set; }

        public string OrderStatus { get; set; }

        public string CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public string Shipper { get; set; }

        public DateTime? ShippedDate { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }
    }
}
