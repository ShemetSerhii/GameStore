using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsQuoted { get; set; }

        public string CrossProperty { get; set; }

        public string Name { get; set; }

        public string Body { get; set; }

        public int Order { get; set; }

        public int? ParentId { get; set; }

        [BsonIgnore]
        public virtual Comment ParentComment { get; set; }

        [BsonIgnore]
        public virtual ICollection<Comment> ChildrenComments { get; set; }

        public int? GameId { get; set; }

        [BsonIgnore]
        public virtual Game Game { get; set; }

        public Comment()
        {
            ChildrenComments = new List<Comment>();
        }
    }
}
