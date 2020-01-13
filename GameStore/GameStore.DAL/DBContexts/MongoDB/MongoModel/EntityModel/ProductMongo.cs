using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace GameStore.DAL.DBContexts.MongoDB.MongoModel
{
    [BsonIgnoreExtraElements]
    public class ProductMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int ProductID { get; set; }
        public bool? IsDeleted { get; set; }
        public string Key { get; set; }
        public string ProductName { get; set; }
        public int SupplierID { get; set; }
        public int CategoryID { get; set; }
        public string QuantityPerUnit { get; set; }
        public double UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int UnitsOnOrder { get; set; }
        public int ReorderLevel { get; set; }
        public int Discontinued { get; set; }
        public int Visits { get; set; }
        public DateTime DatePublication { get; set; }
        public DateTime DateAdded { get; set; }
        public string Publisher { get; set; }
        public ICollection<string> Genres { get; set; }
        public ICollection<string> PlatformTypes { get; set; }

        public ProductMongo()
        {
            Genres = new List<string>();
            PlatformTypes = new List<string>();
        }
    }
}
