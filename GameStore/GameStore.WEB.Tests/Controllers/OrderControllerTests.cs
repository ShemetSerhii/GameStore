using System;
using GameStore.BLL.Interfaces;
using GameStore.WEB.Controllers;
using GameStore.WEB.Models;
using GameStore.WEB.Tests.Tools;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GameStore.Domain.Entities;
using GameStore.WEB.Services.Payment;

namespace GameStore.WEB.Tests.Controllers
{
    [TestFixture]
    public class OrderControllerTests
    {
        private Mock<IOrderService> _orderMock;
        private HttpContextManager _contextManager;

        [Test]
        public void GetBasket_Always_ReturnModelListOrderDetailViewModel()
        {
            _contextManager = new HttpContextManager();
            var controller = new OrderController(_orderMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _contextManager.GetMockedHttpContext()
                }
            };

            var result = controller.GetBasket();

            Assert.AreEqual(result.Model.GetType(), typeof(List<OrderDetailViewModel>));
        }

        [Test]
        public void Delete_WhenSessionContainOneOrderDetailAndParameterIsGameKey_DeleteOrderDetailFromSession()
        {
            _contextManager = new HttpContextManager();
            var controller = new OrderController(_orderMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _contextManager.GetMockedHttpContext(),
                }
            };

            controller.Delete(1);
            var result = controller.Session["basket"] as Order;

            Assert.AreEqual(result.OrderDetails.Count, 0);
        }

        [Test]
        public void GetTotalNumbers_Always_ReturnNumberOfGamesInSession()
        {
            _contextManager = new HttpContextManager();
            var controller = new OrderController(_orderMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _contextManager.GetMockedHttpContext(),
                }
            };

            var result = controller.GetTotalNumbers();

            Assert.AreEqual(result.Model, "1");
        }

        [Test]
        public void MakeOrder_WhenOrderIsNew_CallCreateActionFromService()
        {
            _contextManager = new HttpContextManager();
            var controller = new OrderController(_orderMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _contextManager.GetMockedHttpContext(),
                }
            };

            _orderMock.Reset();
            _orderMock.Setup(m => m.GetLastOrder("User")).Returns(new Order { OrderDate = DateTime.UtcNow.AddHours(-2) });
            _orderMock.Setup(m => m.GetGame("Key")).Returns(new Game());

            controller.MakeOrder();

            _orderMock.Verify(m => m.CreateOrder(It.IsAny<Order>()), Times.Once);
        }

        [Test]
        public void MakeOrder_WhenOrderAlreadyCreated_DoNotCallCreateActionFromService()
        {
            _contextManager = new HttpContextManager();
            var controller = new OrderController(_orderMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _contextManager.GetMockedHttpContext(),
                }
            };

            _orderMock.Reset();
            _orderMock.Setup(m => m.GetLastOrder("User")).Returns(new Order { OrderDate = DateTime.UtcNow });

            controller.MakeOrder();

            _orderMock.Verify(m => m.CreateOrder(It.IsAny<Order>()), Times.Never);
        }

        [Test]
        public void ChoosePayment_WhenChooseBankPayment_ReturnFileStreamResult()
        {
            _contextManager = new HttpContextManager();
            var controller = new OrderController(_orderMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _contextManager.GetMockedHttpContext(),
                }
            };

            var result = controller.ChoosePayment(PaymentEnum.BankPayment);

            Assert.AreEqual(result.GetType(), typeof(FileStreamResult));
        }

        [Test]
        public void ChoosePayment_WhenChooseVisaPayment_ReturnViewResult()
        {
            _contextManager = new HttpContextManager();
            var controller = new OrderController(_orderMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _contextManager.GetMockedHttpContext(),
                }
            };
            
            var result = controller.ChoosePayment(PaymentEnum.VisaPayment);

            Assert.AreEqual(result.GetType(), typeof(ViewResult));
        }

        [Test]
        public void AddIntoBasket_WhenAddIntoOrderAnotherGame_AddGameIntoOrder()
        {
            _contextManager = new HttpContextManager();
            var controller = new OrderController(_orderMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _contextManager.GetMockedHttpContext(),
                }
            };

            controller.AddIntoBasket("Game");
            var result = controller.Session["basket"] as Order;

            Assert.AreEqual(result.OrderDetails.Count, 2);
        }

        [Test]
        public void AddIntoBasket_WhenGameAlreadyExistsIntoOrder_IncreaseQuantity()
        {
            _contextManager = new HttpContextManager();
            var controller = new OrderController(_orderMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _contextManager.GetMockedHttpContext(),
                }
            };

            controller.AddIntoBasket("Key");
            var result = controller.Session["basket"] as Order;

            Assert.AreEqual(result.OrderDetails.First().Quantity, 2);
        }

        [SetUp]
        public void Init()
        {
            _orderMock = new Mock<IOrderService>();

            _orderMock.Setup(m => m.GetGame("Game")).Returns(new Game
            {
                Id = 1,
                Key = "Game",
                Price = 100
            });

            _orderMock.Setup(m => m.GetGame("Key")).Returns(new Game
            {
                Id = 1,
                Key = "Key",
                Price = 100
            });
        }
    }
}
