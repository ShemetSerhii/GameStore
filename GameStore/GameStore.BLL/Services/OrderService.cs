using GameStore.BLL.Interfaces;
using GameStore.BLL.Services.Tools;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GameStore.BLL.Services
{
    public class OrderService : IOrderService
    {
       
        private IUnitOfWork unitOfWork { get; }

        public  Dictionary<OrderStatuses, string> Statuses { get; }

        public OrderService(IUnitOfWork uow)
        {
            unitOfWork = uow;

            Statuses = new Dictionary<OrderStatuses, string>
            {
                {OrderStatuses.NotPaid, "Not paid"},
                {OrderStatuses.Paid, "Paid" },
                {OrderStatuses.Shipped, "Shipped" }
            };
        }

        public Order Get(int id)
        {
            var order = unitOfWork.Orders.Get(x => x.Id == id).SingleOrDefault();

            return order;
        }

        public Order GetLastOrder(string customer)
        {
            var orders = unitOfWork.Orders.Get(
                x => x.CustomerId != null && x.CustomerId == customer);

            if (orders.Any())
            {
                return orders.Last();
            }

            return null;
        }

        public Order GetOrderByInterimProperty(string crossId)
        {
            if (int.TryParse(crossId, out int id))
            {
                var order = unitOfWork.Orders.Get(x => x.Id == id).SingleOrDefault();

                SetupMongoProduct(order.OrderDetails);

                return order;
            }
            else
            {
                var order = unitOfWork.Orders.Get(x => x.CrossId == crossId).SingleOrDefault();

                return order;
            }
        }

        public IEnumerable<Order> GetOrdersLog(DateTime timeFrom, DateTime timeTo)
        {
            var orders = unitOfWork.Orders.Get(x => x.OrderDate >= timeFrom && x.OrderDate <= timeTo.AddDays(1));

            return orders;
        }

        public Game GetGame(string key)
        {
            var game = unitOfWork.Games.Get(
                g => g.Key == key).SingleOrDefault();

            ChangeCurrentCurrency(game);

            return game;
        }

        public void CreateOrder(Order order)
        {
            order.OrderStatus = Statuses[OrderStatuses.NotPaid];

            unitOfWork.Orders.Create(order);
        }

        public void ChangeOrderStatus(Order orderSession)
        {
            if (orderSession != null)
            {
                var order = GetLastOrder(orderSession.CustomerId);

                order.OrderStatus = Statuses[OrderStatuses.Paid];

                unitOfWork.Orders.Update(order);
            }
        }

        public void Update(Order order)
        {
            unitOfWork.Orders.Update(order);
        }

        public void Delete(int id)
        {
            var order = unitOfWork.Orders.Get(x => x.Id == id).SingleOrDefault();

            unitOfWork.Orders.Remove(order);
        }

        private void SetupMongoProduct(IEnumerable<OrderDetail> orderDetails)
        {
            foreach (var detail in orderDetails)
            {
                if (detail.CrossKey != null)
                {
                    detail.Game = unitOfWork.Games.Get(x => x.Key == detail.CrossKey).SingleOrDefault();
                }
            }
        }

        private void ChangeCurrentCurrency(Game game)
        {
            if (Thread.CurrentThread.CurrentCulture.Name == "ru-RU")
            {
                game.Price *= CurrencyApiReader.CurrencyRU;

                game.Price = decimal.Round(game.Price, 2);
            }
        }
    }
}
