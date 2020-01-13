using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GameStore.DAL.DBContexts.EF.Repositories
{
    public class SqlOrderRepository : IRepository<Order>
    {
        private readonly SqlContext _context;

        public SqlOrderRepository(SqlContext context)
        {
            _context = context;
        }

        public void Create(Order item)
        {
            _context.Orders.Add(item);

            _context.SaveChanges();
        }

        public IEnumerable<Order> Get()
        {
            return _context.Orders.Where(x => x.IsDeleted == false);
        }

        public IEnumerable<Order> Get(Func<Order, bool> predicate, 
            Func<IEnumerable<Order>, IOrderedEnumerable<Order>> sorting = null)
        {
            var orders = _context.Orders.Where(predicate);

            orders = SeparationOfDeleted(orders);

            SetupCrossId(orders);

            return orders;
        }

        public void Remove(Order item)
        {
            item.IsDeleted = true;

            Update(item);
        }

        public void Update(Order item)
        {
            if (_context.Entry(item).State == EntityState.Detached)
            {
                _context.Orders.Attach(item);
            }

            _context.Entry(item).State = EntityState.Modified;

            _context.SaveChanges();
        }

        private IEnumerable<Order> SeparationOfDeleted(IEnumerable<Order> orders)
        {
            return orders.Where(x => x.IsDeleted == false);
        }

        private void SetupCrossId(IEnumerable<Order> orders)
        {
            foreach (var order in orders)
            {
                order.CrossId = order.Id.ToString();
            }
        }
    }
}
