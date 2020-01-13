using GameStore.BLL.Interfaces;
using GameStore.BLL.Services.Tools;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public Dictionary<OrderStatuses, string> Statuses { get; }

        public OrderService(IUnitOfWork uow)
        {
            _unitOfWork = uow;

            Statuses = new Dictionary<OrderStatuses, string>
            {
                {OrderStatuses.NotPaid, "Not paid"},
                {OrderStatuses.Paid, "Paid" },
                {OrderStatuses.Shipped, "Shipped" }
            };
        }

        public Task<Order> Get(int id)
        {
            return _unitOfWork.OrderRepository.GetAsync(id);
        }

        public async Task<Order> GetLastOrder(string customer)
        {
            var orders = await _unitOfWork.OrderRepository.GetAsync(
                x => x.CustomerId != null && x.CustomerId == customer);

            if (orders.Any())
            {
                return orders.Last();
            }

            return null;
        }

        public Task<IEnumerable<Order>> GetOrdersLog(DateTime timeFrom, DateTime timeTo)
        {
            return _unitOfWork.OrderRepository.GetAsync(x => x.OrderDate >= timeFrom && x.OrderDate <= timeTo.AddDays(1));
        }

        public Task CreateOrder(Order order)
        {
            order.OrderStatus = Statuses[OrderStatuses.NotPaid];

            _unitOfWork.OrderRepository.Create(order);

            return _unitOfWork.SaveAsync();
        }

        public async Task ChangeOrderStatus(Order orderSession)
        {
            if (orderSession != null)
            {
                var order = await GetLastOrder(orderSession.CustomerId);

                order.OrderStatus = Statuses[OrderStatuses.Paid];

                _unitOfWork.OrderRepository.Update(order);

                await _unitOfWork.SaveAsync();
            }
        }

        public Task Update(Order order)
        {
            _unitOfWork.OrderRepository.Update(order);

            return _unitOfWork.SaveAsync();
        }

        public async Task Delete(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetAsync(id);

            _unitOfWork.OrderRepository.Delete(order);
        }
    }
}
