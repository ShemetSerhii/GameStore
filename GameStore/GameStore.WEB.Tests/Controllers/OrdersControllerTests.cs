using GameStore.BLL.Interfaces;
using GameStore.BLL.Services.Tools;
using GameStore.Domain.Entities;
using GameStore.WEB.Controllers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using GameStore.WEB.Models.DomainViewModel.EditorModels;

namespace GameStore.WEB.Tests.Controllers
{
    [TestFixture]
    public class OrdersControllerTests
    {
        private Mock<IOrderService> _orderMock;
        private Mock<IShipperService> _shipperMock;

        [Test]
        public void Index_Always_CallGetOrderLogActionFromService()
        {
            var controller = new OrdersController(_orderMock.Object, _shipperMock.Object);

            controller.Index();

            _orderMock.Verify(m => m.GetOrdersLog(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Test]
        public void Cancel_WhenParameterIsId_CallGetOrderLogActionFromService()
        {
            var controller = new OrdersController(_orderMock.Object, _shipperMock.Object);

            controller.Cancel(1);

            _orderMock.Verify(m => m.Delete(1));
        }

        [Test]
        public void History_Always_CallGetOrderLogActionFromService()
        {
            var controller = new OrdersController(_orderMock.Object, _shipperMock.Object);

            controller.History();

            _orderMock.Verify(m => m.GetOrdersLog(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Test]
        public void GetOrderDetails_WhenParameterIsCrossId_CallGetCrossOrderActionFromService()
        {
            var controller = new OrdersController(_orderMock.Object, _shipperMock.Object);

            controller.GetOrderDetails("1");

            _orderMock.Verify(m => m.GetOrderByInterimProperty("1"), Times.Once);
        }

        [Test]
        public void GetOrders_WhenParameterDateLine_CallGetOrdersLogActionFromService()
        {
            var controller = new OrdersController(_orderMock.Object, _shipperMock.Object);

            controller.Orders(DateTime.UtcNow.AddDays(-1).ToString(), DateTime.UtcNow.ToString());

            _orderMock.Verify(m => m.GetOrdersLog(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Test]
        public void GetHistoryLog_WhenParameterDateLine_CallGetOrdersLogActionFromService()
        {
            var controller = new OrdersController(_orderMock.Object, _shipperMock.Object);

            controller.GetHistoryLog(DateTime.UtcNow.AddDays(-1).ToString(), DateTime.UtcNow.ToString());

            _orderMock.Verify(m => m.GetOrdersLog(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Test]
        public void Get_Edit_WhenParameterIsId_CallGetActionFromService()
        {
            var controller = new OrdersController(_orderMock.Object, _shipperMock.Object);

            controller.Edit(1);

            _orderMock.Verify(m => m.Get(1), Times.Once);
        }

        [Test]
        public void Post_Edit_WhenParameterIsId_CallGetActionFromService()
        {
            var controller = new OrdersController(_orderMock.Object, _shipperMock.Object);
            var orderModel = new OrderEditorModel
            {
                OrderModel = new OrderEditModel { Id = 1, CustomerId = "Customer"}
            };
            var selectShipper = new[] {"Shipper"};
            var selectStatus = new[] {"Paid"};

            controller.Edit(orderModel, selectShipper, selectStatus);

            _orderMock.Verify(m => m.Update(It.Is<Order>(o => o.CustomerId == "Customer")), Times.Once);
        }

        [SetUp]
        public void Init()
        {
            _orderMock = new Mock<IOrderService>();
            _shipperMock = new Mock<IShipperService>();

            _orderMock.Setup(m => m.GetOrdersLog(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new List<Order>
            {
                new Order(),
                new Order()
            });

            _orderMock.Setup(m => m.GetOrderByInterimProperty("1")).Returns(new Order { CrossId = "1" });

            _orderMock.Setup(m => m.Get(1)).Returns(new Order {OrderStatus = "Paid" , Shipper = "Shipper"});

            _orderMock.Setup(m => m.Statuses).Returns(new Dictionary<OrderStatuses, string>
            {
                {OrderStatuses.Paid, "Paid"}
            });

            _shipperMock.Setup(m => m.GetAll()).Returns(new List<Shipper>
            {
                new Shipper
                {
                    Id = 1,
                    CompanyName = "Name",
                    Phone = "12345"
                }
            });
        }
    }
}
