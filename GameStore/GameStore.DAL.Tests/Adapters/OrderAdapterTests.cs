using System;
using GameStore.DAL.Adapters;
using GameStore.DAL.DBContexts.MongoDB.Intefaces;
using GameStore.DAL.DBContexts.MongoDB.Logging.Interfaces;
using GameStore.DAL.DBContexts.MongoDB.Logging.LogEntity;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace GameStore.DAL.Tests.Adapters
{
    [TestFixture]
    public class OrderAdapterTests
    {
        private Mock<IGenericRepository<Order>> _sqlMock;
        private Mock<IMongoRepository<Order>> _mongoMock;
        private Mock<ILogging> _loggerMock;

        [Test]
        public void Create_WhenItemNotNull_CallCreateActionFromSqlRepository()
        {
            var adapter = new OrderAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            var order = new Order();

            adapter.Create(order);

            _sqlMock.Verify(x => x.Create(order), Times.Once);
        }

        [Test]
        public void Update_WhenItemNotNull_CallUpdateActionFromSqlRepository()
        {
            var adapter = new OrderAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            var order = new Order();

            adapter.Update(order);

            _sqlMock.Verify(x => x.Update(order), Times.Once);
        }

        [Test]
        public void Remove_WhenItemNotNull_CallRemoveActionFromSqlRepository()
        {
            var adapter = new OrderAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            var order = new Order();

            adapter.Remove(order);

            _sqlMock.Verify(x => x.Remove(order), Times.Once);
        }

        [Test]
        public void Get_Always_CallGetActionFromSqlRepository()
        {
            var adapter = new OrderAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            adapter.Get();

            _sqlMock.Verify(m => m.Get(), Times.Once);
        }

        [Test]
        public void Get_WhenPredicateNotNull_CallGetActionFromBothRepositories()
        {
            var adapter = new OrderAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            adapter.Get(x => x.Id == 1);

            _sqlMock.Verify(m => m.Get(It.IsAny<Func<Order, bool>>(), null), Times.Once);
            _mongoMock.Verify(m => m.Get(It.IsAny<Func<Order, bool>>(), null), Times.Once);
        }

        [SetUp]
        public void Init()
        {
            _sqlMock = new Mock<IGenericRepository<Order>>();
            _mongoMock = new Mock<IMongoRepository<Order>>();
            _loggerMock = new Mock<ILogging>();

            _loggerMock.Setup(x => x.CudDictionary).Returns(new Dictionary<CUDEnum, string>
            {
                {CUDEnum.Update, "Update" },
                {CUDEnum.Delete, "Delete" },
                {CUDEnum.Create, "Create" }
            });

            _sqlMock.Setup(m => m.Get(It.IsAny<Func<Order, bool>>(), null)).Returns(new List<Order>
            {
                new Order()
            });

            _mongoMock.Setup(m => m.Get(It.IsAny<Func<Order, bool>>(), null)).Returns(new List<Order>
            {
                new Order()
            });
        }
    }
}
