using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GameStore.DAL.DBContexts.MongoDB.MongoModel
{
    [BsonIgnoreExtraElements]
    public class SupplierMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public bool? IsDeleted { get; set; }
        public int SupplierID { get; set; }
        public int PublisherId { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public object Address { get; set; }
        public object City { get; set; }
        public string Region { get; set; }
        public object PostalCode { get; set; }
        public string Country { get; set; }
        public object Phone { get; set; }
        public object Fax { get; set; }
        public string HomePage { get; set; }
        public ICollection<int> SqlGamesId { get; set; }

        public SupplierMongo()
        {
            SqlGamesId = new List<int>();
        }
    }
}
