using GameStore.BLL.Services;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GameStore.BLL.Tests.Services
{
    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IUnitOfWork> _mock;

        [Test]
        public void GetCrossOrder_WhenParameterIsCrossIdParseToInt_CallGetActionWithPredicateFromAdapter()
        {
            var service = new OrderService(_mock.Object);

            service.GetOrderByInterimProperty("1");

            _mock.Verify(x => x.Orders.Get(It.IsAny<Func<Order, bool>>(), null), Times.Once);
        }

        [Test]
        public void GetCrossOrder_WhenParameterIsCrossIdNotParseToInt_CallGetActionWithPredicateFromAdapter()
        {
            var service = new OrderService(_mock.Object);

            service.GetOrderByInterimProperty("Cross");

            _mock.Verify(x => x.Orders.Get(It.IsAny<Func<Order, bool>>(), null), Times.Once);
        }

        [Test]
        public void GetOrdersLog_WhenParameterIsTimeLine_CallGetActionWithPredicateFromAdapter()
        {
            var service = new OrderService(_mock.Object);

            service.GetOrdersLog(DateTime.UtcNow.AddDays(-2), DateTime.UtcNow);

            _mock.Verify(x => x.Orders.Get(It.IsAny<Func<Order, bool>>(), null), Times.Once);
        }

        [Test]
        public void GetGame_WhenParameterIsGameKey_CallGetActionWithPredicateFromGameAdapter()
        {
            var service = new OrderService(_mock.Object);

            service.GetGame("key");

            _mock.Verify(x => x.Games.Get(It.IsAny<Func<Game, bool>>(), null), Times.Once);
        }

        [Test]
        public void CreateOrder_WhenItemIsNotNull_CallCreateActionFromAdapter()
        {
            var service = new OrderService(_mock.Object);
            var order = new Order { Id = 1 };

            service.CreateOrder(order);

            _mock.Verify(x => x.Orders.Create(order), Times.Once);
        }

        [Test]
        public void Update_WhenItemIsNotNull_CallUpdateActionFromAdapter()
        {
            var service = new OrderService(_mock.Object);
            var order = new Order { Id = 1 };

            service.Update(order);

            _mock.Verify(x => x.Orders.Update(order), Times.Once);
        }

        [Test]
        public void Get_WhenParameterIsId_CallGetActionFromAdapter()
        {
            var service = new OrderService(_mock.Object);

            service.Get(1);

            _mock.Verify(x => x.Orders.Get(It.IsAny<Func<Order, bool>>(), null), Times.Once);
        }

        [Test]
        public void GetLastOrder_WhenParameterIsCustomerLogin_CallGetActionFromAdapter()
        {
            var service = new OrderService(_mock.Object);

            service.GetLastOrder("login");

            _mock.Verify(x => x.Orders.Get(It.IsAny<Func<Order, bool>>(), null), Times.Once);
        }

        [Test]
        public void Delete_WhenParameterIsId_CallDeleteActionFromAdapter()
        {
            var service = new OrderService(_mock.Object);

            service.Delete(1);

            _mock.Verify(x => x.Orders.Remove(It.Is<Order>(o => o.Id == 1)), Times.Once);
        }

        [Test]
        public void ChangeOrderStatus_WhenItemNotNull_SetPaidStatus()
        {
            var service = new OrderService(_mock.Object);
            var order = new Order
            {
                CustomerId = "login"
            };

            service.ChangeOrderStatus(order);

            _mock.Verify(x => x.Orders.Update(It.Is<Order>(o => o.OrderStatus == "Paid")), Times.Once);
        }

        [SetUp]
        public void Init()
        {
            _mock = new Mock<IUnitOfWork>();

            _mock.Setup(x => x.Orders.Get(It.IsAny<Func<Order, bool>>(), null)).Returns(new List<Order>
            {
                new Order
                {
                    Id = 1,
                    OrderDate = DateTime.UtcNow,
                    OrderDetails = new List<OrderDetail>
                    {
                        new OrderDetail
                        {
                            Game = new Game {Key = "Key"},
                            CrossKey = "Key"
                        }
                    },
                    CustomerId = "login",
                    Shipper = "Shipper",
                    ShippedDate = DateTime.UtcNow
                }
            });

            _mock.Setup(x => x.OrderDetails.Get(It.IsAny<Func<OrderDetail, bool>>(), null)).Returns(new List<OrderDetail>
            {
                new OrderDetail
                {
                    OrderId = 1,
                    GameId = 1,
                    Discount = 1,
                    Order = new Order(),
                    IsDeleted = false,
                    Game = new Game {Key = "Key"},
                    CrossKey = "Key"
                }
            });

            _mock.Setup(x => x.Games.Get(It.IsAny<Func<Game, bool>>(), null)).Returns(new List<Game>
            {
                new Game()
            });
        }
    }
}
