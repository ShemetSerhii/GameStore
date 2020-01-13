using System;
using System.ComponentModel.DataAnnotations;

namespace GameStore.WEB.Models.DomainViewModel.EditorModels
{
    public class OrderEditModel
    {
        public int Id { get; set; }

        public string OrderStatus { get; set; }

        [Display(ResourceType = typeof(Resources.Orders.OrdersResource), Name = "Customer")]
        public string CustomerId { get; set; }

        [Display(ResourceType = typeof(Resources.Orders.OrdersResource), Name = "OrderDate")]
        public DateTime OrderDate { get; set; }

        [Display(ResourceType = typeof(Resources.Orders.OrdersResource), Name = "Shipper")]
        public string Shipper { get; set; }

        [Display(ResourceType = typeof(Resources.Orders.OrdersResource), Name = "ShippedDate")]
        public string ShippedDate { get; set; }     
    }
}