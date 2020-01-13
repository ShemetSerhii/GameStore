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
    public class OrderDetailAdapter : IAdapter<OrderDetail>
    {
        private readonly IGenericRepository<OrderDetail> _sql;
        private readonly ILogging _logging;

        public OrderDetailAdapter(IGenericRepository<OrderDetail> sqlOrderDetail, ILogging logging)
        {
            _sql = sqlOrderDetail;
            _logging = logging;
        }

        public void Create(OrderDetail item)
        {
            _sql.Create(item);

            _logging.Log(item.GetType(), _logging.CudDictionary[CUDEnum.Create], item.ToBsonDocument());
        }

        public IEnumerable<OrderDetail> Get()
        {
            var orderDetail = _sql.Get();

            return orderDetail;
        }

        public IEnumerable<OrderDetail> Get(Func<OrderDetail, bool> predicate, 
            Func<IEnumerable<OrderDetail>, IOrderedEnumerable<OrderDetail>> sorting = null)
        {
            var query = _sql.Get(predicate, sorting);

            return query.ToList();
        }

        public void Remove(OrderDetail item)
        {
            _sql.Remove(item);
        }

        public void Update(OrderDetail item)
        {
           _sql.Update(item);
        }
    }
}
