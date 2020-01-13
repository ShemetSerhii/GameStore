using GameStore.Domain.Entities.Interfaces;
using System.Collections.Generic;

namespace GameStore.Domain.Entities
{
    public class Genre : Entity
    {
        public bool IsDeleted { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        public Genre Parent { get; set; }

        public ICollection<Game> Games { get; set; }

        public ICollection<Genre> ChildrenGenres { get; set; }

    }
}
