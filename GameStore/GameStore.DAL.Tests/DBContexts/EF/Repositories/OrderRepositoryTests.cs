using GameStore.DAL.DBContexts.EF.Repositories;
using GameStore.DAL.EF;
using GameStore.Domain.Entities;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GameStore.DAL.DBContexts.EF;

namespace GameStore.DAL.Tests.DBContexts.EF.Repositories
{
    [TestFixture]
    class OrderRepositoryTests
    {
        private Mock<SqlContext> _contextMock;
        private Mock<DbSet<Order>> _entitiesMock;

        [SetUp]
        public void Init()
        {
            _contextMock = new Mock<SqlContext>();
            _entitiesMock = new Mock<DbSet<Order>>();
        }

        [Test]
        public void OrderRepository_Add_CallsAddActionFromAppDbContext()
        {
            var order = new Order() { Id = 1 };

            _entitiesMock.Setup(x => x.Add(order));
            _contextMock.Setup(x => x.Set<Order>()).Returns(_entitiesMock.Object);

            var repository = new SqlOrderRepository(_contextMock.Object);
            repository.Create(order);

            _entitiesMock.Verify(x => x.Add(order), Times.Once);
        }

        [Test]
        public void Get_WhenPredicateIsNull_ReturnAllOrders()
        {
            SetMockDbSet();

            _contextMock.Setup(x => x.Set<Order>()).Returns(_entitiesMock.Object);
            var repository = new SqlOrderRepository(_contextMock.Object);

            var result = repository.Get();

            Assert.AreEqual(result.Count(), 2);
        }

        private void SetMockDbSet()
        {
            var orders = new List<Order>
            {
                new Order() { Id = 1, CustomerId = "1" },
                new Order() { Id = 2, CustomerId = "2" }
            }.AsQueryable();

            _entitiesMock.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(orders.Provider);
            _entitiesMock.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(orders.Expression);
            _entitiesMock.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(orders.ElementType);
            _entitiesMock.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(orders.GetEnumerator());
        }
    }
}
