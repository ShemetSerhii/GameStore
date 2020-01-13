using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.Domain.Entities
{
    public class GameTranslate
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int GameId { get; set; }

        [BsonIgnore]
        public virtual Game Game { get; set; }

        public string Language { get; set; }
    }
}
