using GameStore.DAL.DBContexts.MongoDB.Intefaces;
using GameStore.DAL.DBContexts.MongoDB.Logging.Interfaces;
using GameStore.DAL.DBContexts.MongoDB.Logging.LogEntity;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.Adapters
{
    public class OrderAdapter : IAdapter<Order>
    {
        private readonly IGenericRepository<Order> _sql;
        private readonly IMongoRepository<Order> _mongo;
        private readonly ILogging _logging;

        public OrderAdapter(IGenericRepository<Order> orderSql, IMongoRepository<Order> orderMongo, ILogging logging)
        {
            _sql = orderSql;
            _mongo = orderMongo;
            _logging = logging;
        }

        public void Create(Order item)
        {
            _sql.Create(item);

            _logging.Log(item.GetType(), _logging.CudDictionary[CUDEnum.Create], item.ToBsonDocument());
        }

        public IEnumerable<Order> Get()
        {
            var orders = _sql.Get();

            return orders;
        }

        public IEnumerable<Order> Get(Func<Order, bool> predicate, 
            Func<IEnumerable<Order>, IOrderedEnumerable<Order>> sorting = null)
        {
            var query = _sql.Get(predicate).ToList();

            query.AddRange(_mongo.Get(predicate));

            return query.OrderBy(x => x.OrderDate).ToList();
        }

        public void Remove(Order item)
        {
            _sql.Remove(item);
        }

        public void Update(Order item)
        {
            _sql.Update(item);

            _logging.Log(item.GetType(), _logging.CudDictionary[CUDEnum.Update], item.ToBsonDocument());
        }
    }
}
