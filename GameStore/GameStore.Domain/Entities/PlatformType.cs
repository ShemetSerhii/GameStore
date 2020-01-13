using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.Domain.Entities
{
    public class PlatformType
    {
        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(100)]
        public string Type { get; set; }

        [BsonIgnore]
        public virtual ICollection<Game> Games { get; set; }

        public PlatformType()
        {
            Games = new List<Game>();
        }
    }
}
