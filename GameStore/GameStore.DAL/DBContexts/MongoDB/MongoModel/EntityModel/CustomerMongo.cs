using MongoDB.Bson;

namespace GameStore.DAL.DBContexts.MongoDB.MongoModel.EntityModel
{
    public class CustomerMongo
    {
        public ObjectId Id { get; set; }
        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public object Address { get; set; }
        public object City { get; set; }
        public string Region { get; set; }
        public object PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }
}
