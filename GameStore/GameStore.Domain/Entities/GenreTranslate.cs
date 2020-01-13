
namespace GameStore.Domain.Entities
{
    public class GenreTranslate
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; }

        public string Language { get; set; }
    }
}
