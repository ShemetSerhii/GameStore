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

        public string Name { get; set; }

        public string Body { get; set; }

        public int Order { get; set; }

        public int? ParentId { get; set; }

        public Comment ParentComment { get; set; }

        public ICollection<Comment> ChildrenComments { get; set; }

        public int? GameId { get; set; }

        public  Game Game { get; set; }
    }
}
