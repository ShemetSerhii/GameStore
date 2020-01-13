using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.DBContexts.MongoDB.MongoModel
{
    public class ShipperMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int ShipperID { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
    }
}
