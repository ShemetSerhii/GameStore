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

        public OrderAdapter(IGenericRepository<Order> orderSql)
        {
            _sql = orderSql;
        }

        public void Create(Order item)
        {
            _sql.Create(item);
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

            return query.OrderBy(x => x.OrderDate).ToList();
        }

        public void Remove(Order item)
        {
            _sql.Remove(item);
        }

        public void Update(Order item)
        {
            _sql.Update(item);
        }
    }
}
