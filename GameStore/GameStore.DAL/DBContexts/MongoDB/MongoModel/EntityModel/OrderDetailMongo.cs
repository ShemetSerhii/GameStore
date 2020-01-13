using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.DBContexts.MongoDB.MongoModel
{
    public class OrderDetailMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public object Discount { get; set; }
    }
}
