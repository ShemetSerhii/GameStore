using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Entities
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public string CrossProperty { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        [BsonIgnore]
        public virtual Genre Parent { get; set; }

        [BsonIgnore]
        public virtual ICollection<GenreTranslate> GenreTranslates { get; set; }

        [BsonIgnore]
        public virtual ICollection<Game> Games { get; set; }

        [BsonIgnore]
        public virtual ICollection<Genre> ChildrenGenres { get; set; }

        public Genre()
        {
            GenreTranslates = new List<GenreTranslate>();
            Games = new List<Game>();
            ChildrenGenres = new List<Genre>();
        }
    }
}
