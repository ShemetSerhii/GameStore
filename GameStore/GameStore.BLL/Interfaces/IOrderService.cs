using GameStore.BLL.Services.Tools;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.BLL.Interfaces
{
    public interface IOrderService
    {
        Dictionary<OrderStatuses, string> Statuses { get; }

        Task<Order> Get(int id);

        Task<Order> GetLastOrder(string customer);

        Task<IEnumerable<Order>> GetOrdersLog(DateTime timeFrom, DateTime timeTo);

        Task CreateOrder(Order order);

        Task ChangeOrderStatus(Order orderSession);

        Task Update(Order order);

        Task Delete(int id);
    }
}
