using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GameStore.DAL.DBContexts.MongoDB.MongoModel
{
    [BsonIgnoreExtraElements]
    public class CategorieMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public bool? IsDeleted { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Parent { get; set; }
        public ICollection<int> SqlChildrenId { get; set; }
        public ICollection<int> SqlGamesId { get; set; }

        public CategorieMongo()
        {
            SqlGamesId = new List<int>();
            SqlChildrenId = new List<int>();
        }
    }
}
