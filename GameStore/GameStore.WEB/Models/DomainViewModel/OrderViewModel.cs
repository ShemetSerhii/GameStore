using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.WEB.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public string CrossId { get; set; }

        [Display(ResourceType = typeof(Resources.Orders.OrdersResource), Name = "Customer")]
        public string CustomerId { get; set; }

        [Display(ResourceType = typeof(Resources.Orders.OrdersResource), Name = "OrderStatus")]
        public string OrderStatus { get; set; }

        [Display(ResourceType = typeof(Resources.Orders.OrdersResource), Name = "OrderDate")]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        [Display(ResourceType = typeof(Resources.Orders.OrdersResource), Name = "Shipper")]
        public string Shipper { get; set; }

        [Display(ResourceType = typeof(Resources.Orders.OrdersResource), Name = "ShippedDate")]
        [DataType(DataType.Date)]
        public DateTime? ShippedDate { get; set; }

        public ICollection<OrderDetailViewModel> OrderDetails { get; set; }

        public OrderViewModel()
        {
            OrderDetails = new List<OrderDetailViewModel>();
        }
    }
}