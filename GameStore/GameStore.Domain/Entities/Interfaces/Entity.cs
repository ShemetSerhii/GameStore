using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Domain.Entities.Interfaces
{
    public abstract class Entity : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}