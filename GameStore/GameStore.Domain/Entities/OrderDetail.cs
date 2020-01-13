using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.Domain.Entities
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public string CrossKey { get; set; }

        public int? GameId { get; set; }

        [BsonIgnore]
        public virtual Game Game { get; set; }

        public decimal Price { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }

        public int OrderId { get; set; }

        [BsonIgnore]
        public virtual Order Order { get; set; }
    }
}
