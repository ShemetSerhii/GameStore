using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public ICollection<Game> Games { get; set; }
    }
}
