using System;
using GameStore.DAL.DBContexts.MongoDB.Logging.Interfaces;
using GameStore.DAL.DBContexts.MongoDB.Logging.LogEntity;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using GameStore.DAL.Adapters;

namespace GameStore.DAL.Tests.Adapters
{
    [TestFixture]
    public class OrderDetailAdapterTests
    {
        private Mock<IGenericRepository<OrderDetail>> _orderDetailMock;
        private Mock<ILogging> _loggerMock;

        [Test]
        public void Create_WhenItemNotNull_CallCreateActionFromSqlRepository()
        {
            var adapter = new OrderDetailAdapter(_orderDetailMock.Object, _loggerMock.Object);

            var orderDetail = new OrderDetail();

            adapter.Create(orderDetail);

            _orderDetailMock.Verify(x => x.Create(orderDetail), Times.Once);
        }

        [Test]
        public void Update_WhenItemNotNull_CallUpdateActionFromSqlRepository()
        {
            var adapter = new OrderDetailAdapter(_orderDetailMock.Object, _loggerMock.Object);

            var orderDetail = new OrderDetail();

            adapter.Update(orderDetail);

            _orderDetailMock.Verify(x => x.Update(orderDetail), Times.Once);
        }

        [Test]
        public void Remove_WhenItemNotNull_CallRemoveActionFromSqlRepository()
        {
            var adapter = new OrderDetailAdapter(_orderDetailMock.Object, _loggerMock.Object);

            var orderDetail = new OrderDetail();

            adapter.Remove(orderDetail);

            _orderDetailMock.Verify(x => x.Remove(orderDetail), Times.Once);
        }

        [Test]
        public void Get_Always_CallGetActionFromSqlRepository()
        {
            var adapter = new OrderDetailAdapter(_orderDetailMock.Object, _loggerMock.Object);

            adapter.Get();

            _orderDetailMock.Verify(x => x.Get(), Times.Once);
        }

        [Test]
        public void Get_WhenPredicateNotNull_CallGetActionWithPredicateFromSqlRepository()
        {
            var adapter = new OrderDetailAdapter(_orderDetailMock.Object, _loggerMock.Object);

            adapter.Get(It.IsAny<Func<OrderDetail, bool>>());

            _orderDetailMock.Verify(x => x.Get(It.IsAny<Func<OrderDetail, bool>>(), null), Times.Once);
        }

        [SetUp]
        public void Init()
        {
            _orderDetailMock = new Mock<IGenericRepository<OrderDetail>>();
            _loggerMock = new Mock<ILogging>();

            _loggerMock.Setup(x => x.CudDictionary).Returns(new Dictionary<CUDEnum, string>
            {
                {CUDEnum.Update, "Update" },
                {CUDEnum.Delete, "Delete" },
                {CUDEnum.Create, "Create" }
            });
        }
    }
}
