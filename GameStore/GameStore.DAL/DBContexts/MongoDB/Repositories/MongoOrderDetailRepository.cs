using AutoMapper;
using GameStore.DAL.DBContexts.MongoDB.Intefaces;
using GameStore.DAL.DBContexts.MongoDB.MongoModel;
using GameStore.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.DBContexts.MongoDB.Repositories
{
    public class MongoOrderDetailRepository : IMongoRepository<OrderDetail>
    {
        private readonly IMongoContext _mongoContext;

        public MongoOrderDetailRepository(IMongoContext context)
        {
            _mongoContext = context;
        }

        public IEnumerable<OrderDetail> Get()
        {
            var orderDetails = _mongoContext.OrderDetails.AsQueryable().ToList();

            var storeOrderDetails = Mapper.Map<IEnumerable<OrderDetailMongo>, List<OrderDetail>>(orderDetails);

            return storeOrderDetails;
        }

        public IEnumerable<OrderDetail> Get(Func<OrderDetail, bool> predicate, Func<IEnumerable<OrderDetail>, IOrderedEnumerable<OrderDetail>> sorting = null)
        {
            var orderDetails = _mongoContext.OrderDetails.Find(new BsonDocument()).ToList();

            var storeOrderDetails = Mapper.Map<IEnumerable<OrderDetailMongo>, List<OrderDetail>>(orderDetails);

            return storeOrderDetails.Where(predicate);
        }
    }
}
