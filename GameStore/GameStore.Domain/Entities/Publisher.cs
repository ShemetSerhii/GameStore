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

        public string CompanyName { get; set; }

        public string Description { get; set; }

        public string HomePage { get; set; }

        public ICollection<Game> Games { get; set; }
    }
}
