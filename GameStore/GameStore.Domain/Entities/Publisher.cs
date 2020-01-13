using GameStore.Domain.Entities.Interfaces;
using System.Collections.Generic;

namespace GameStore.Domain.Entities
{
    public class Publisher : Entity
    {
        public string UserLogin { get; set; }

        public bool IsDeleted { get; set; }

        public string CompanyName { get; set; }

        public string Description { get; set; }

        public string HomePage { get; set; }

        public ICollection<Game> Games { get; set; }
    }
}
