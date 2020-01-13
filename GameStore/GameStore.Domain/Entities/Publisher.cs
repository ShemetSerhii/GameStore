using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Entities
{
    public class Publisher
    {
        [Key]
        public int Id { get; set; }

        public string UserLogin { get; set; }

        public bool IsDeleted { get; set; }

        public string CrossProperty { get; set; }

        public string CompanyName { get; set; }

        public string Description { get; set; }

        public string HomePage { get; set; }

        [BsonIgnore]
        public virtual ICollection<Game> Games { get; set; }

        public Publisher()
        {
            Games = new List<Game>();
        }
    }
}
