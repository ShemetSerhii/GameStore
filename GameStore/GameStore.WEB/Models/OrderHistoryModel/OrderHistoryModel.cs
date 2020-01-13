using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.WEB.Models.OrderHistoryModel
{
    public class OrderHistoryModel
    {
        public IEnumerable<OrderViewModel> OrderModels { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Orders.OrdersResource), ErrorMessageResourceName = "DateRequiredError")]
        public string TimeFrom { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Orders.OrdersResource), ErrorMessageResourceName = "DateRequiredError")]
        public string TimeTo { get; set; }

        public OrderHistoryModel()
        {
            OrderModels = new List<OrderViewModel>();
        }
    }
}