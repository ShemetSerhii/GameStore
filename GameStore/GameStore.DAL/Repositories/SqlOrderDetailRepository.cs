using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GameStore.DAL.DBContexts.EF.Repositories
{
    public class SqlOrderDetailRepository : IRepository<OrderDetail>
    {
        private readonly SqlContext _context;

        public SqlOrderDetailRepository(SqlContext context)
        {
            _context = context;
        }

        public void Create(OrderDetail item)
        {
            _context.OrderDetails.Add(item);

            _context.SaveChanges();
        }

        public IEnumerable<OrderDetail> Get()
        {
            return _context.OrderDetails.Where(x => x.IsDeleted == false).ToList();
        }

        public IEnumerable<OrderDetail> Get(Func<OrderDetail, bool> predicate,
            Func<IEnumerable<OrderDetail>, IOrderedEnumerable<OrderDetail>> sorting = null)
        {
            var orderDetails = _context.OrderDetails.Where(predicate);

            orderDetails = SeparationOfDeleted(orderDetails);

            return orderDetails;
        }

        public void Remove(OrderDetail item)
        {
            item.IsDeleted = true;

            Update(item);
        }

        public void Update(OrderDetail item)
        {
            if (_context.Entry(item).State == EntityState.Detached)
            {
                _context.OrderDetails.Attach(item);
            }

            _context.Entry(item).State = EntityState.Modified;

            _context.SaveChanges();
        }

        private IEnumerable<OrderDetail> SeparationOfDeleted(IEnumerable<OrderDetail> details)
        {
            return details.Where(x => x.IsDeleted == false);
        }
    }
}
