using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Entities
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        public Genre Parent { get; set; }

        public ICollection<Game> Games { get; set; }

        public ICollection<Genre> ChildrenGenres { get; set; }

    }
}
