using System.ComponentModel.DataAnnotations;

namespace GameStore.WEB.Models
{
    public class OrderDetailViewModel
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public GameViewModel Game { get; set; }

        [Display(ResourceType = typeof(Resources.Order.OrderResource), Name = "Price")]
        public decimal Price { get; set; }

        [Display(ResourceType = typeof(Resources.Order.OrderResource), Name = "Quantity")]
        public short Quantity { get; set; }

        [Display(ResourceType = typeof(Resources.Order.OrderResource), Name = "Discount")]
        public float Discount { get; set; }

        public int OrderId { get; set; }

        public OrderViewModel Order { get; set; }
    }
}