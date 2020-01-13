using GameStore.DAL.DBContexts.MongoDB.Logging.LogEntity;
using GameStore.DAL.DBContexts.MongoDB.MongoModel;
using GameStore.DAL.DBContexts.MongoDB.MongoModel.EntityModel;
using MongoDB.Driver;

namespace GameStore.DAL.DBContexts.MongoDB.Intefaces
{
    public interface IMongoContext
    {
        IMongoCollection<CustomerMongo> Customers { get; }

        IMongoCollection<ProductMongo> Products { get; }
  
        IMongoCollection<CategorieMongo> Categories { get; }
     
        IMongoCollection<OrderDetailMongo> OrderDetails { get; }
     
        IMongoCollection<OrderMongo> Orders { get; }
      
        IMongoCollection<ShipperMongo> Shippers { get; }
        
        IMongoCollection<SupplierMongo> Suppliers { get; }
        
        IMongoCollection<LogModel> EntityLogs { get; }     
    }
}