using GameStore.DAL.DBContexts.MongoDB.Intefaces;
using GameStore.DAL.DBContexts.MongoDB.Logging.LogEntity;
using GameStore.DAL.DBContexts.MongoDB.MongoModel;
using GameStore.DAL.DBContexts.MongoDB.MongoModel.EntityModel;
using MongoDB.Driver;
using System.Configuration;

namespace GameStore.DAL.DBContexts.MongoDB
{
    public class MongoDbContext : IMongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;

            var client = new MongoClient(connectionString);

            _database = client.GetDatabase("SerhiiShemet");
        }

        public IMongoCollection<CustomerMongo> Customers
        {
            get { return _database.GetCollection<CustomerMongo>("customers"); }
        }

        public IMongoCollection<ProductMongo> Products
        {
            get { return _database.GetCollection<ProductMongo>("products"); }
        }

        public IMongoCollection<CategorieMongo> Categories
        {
            get { return _database.GetCollection<CategorieMongo>("categories"); }
        }

        public IMongoCollection<OrderDetailMongo> OrderDetails
        {
            get { return _database.GetCollection<OrderDetailMongo>("order-details"); }
        }

        public IMongoCollection<OrderMongo> Orders
        {
            get { return _database.GetCollection<OrderMongo>("orders"); }
        }

        public IMongoCollection<ShipperMongo> Shippers
        {
            get { return _database.GetCollection<ShipperMongo>("shippers"); }
        }

        public IMongoCollection<SupplierMongo> Suppliers
        {
            get { return _database.GetCollection<SupplierMongo>("suppliers"); }
        }

        public IMongoCollection<LogModel> EntityLogs
        {
            get { return _database.GetCollection<LogModel>("entity-logs"); }
        }
    }
}
