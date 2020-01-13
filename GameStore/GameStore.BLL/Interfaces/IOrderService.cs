using GameStore.BLL.Services.Tools;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;

namespace GameStore.BLL.Interfaces
{
    public interface IOrderService
    {
        Dictionary<OrderStatuses, string> Statuses { get; }
        Order Get(int id);
        Order GetLastOrder(string customer);
        Order GetOrderByInterimProperty(string interimProperty);
        IEnumerable<Order> GetOrdersLog(DateTime timeFrom, DateTime timeTo);
        Game GetGame(string key);
        void CreateOrder(Order order);
        void ChangeOrderStatus(Order orderSession);
        void Update(Order order);
        void Delete(int id);
    }
}
