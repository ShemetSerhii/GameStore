using GameStore.DAL.DBContexts.EF;
using GameStore.DAL.DBContexts.EF.Repositories;
using GameStore.Domain.Entities;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GameStore.DAL.Tests.DBContexts.EF.Repositories
{
    [TestFixture]
    class OrderDetailRepositoryTests
    {
        private Mock<SqlContext> _contextMock;
        private Mock<DbSet<OrderDetail>> _entitiesMock;

        [SetUp]
        public void Init()
        {
            _contextMock = new Mock<SqlContext>();
            _entitiesMock = new Mock<DbSet<OrderDetail>>();
        }

        [Test]
        public void OrderDetailRepository_Add_CallsAddActionFromAppDbContext()
        {
            var orderDetail = new OrderDetail() { Id = 1 };

            _entitiesMock.Setup(x => x.Add(orderDetail));
            _contextMock.Setup(x => x.Set<OrderDetail>()).Returns(_entitiesMock.Object);

            var repository = new SqlOrderDetailRepository(_contextMock.Object);
            repository.Create(orderDetail);

            _entitiesMock.Verify(x => x.Add(orderDetail), Times.Once);
        }

        [Test]
        public void Get_WhenPredicateIsNull_ReturnAllOrderDetails()
        {
            SetMockDbSet();

            _contextMock.Setup(x => x.Set<OrderDetail>()).Returns(_entitiesMock.Object);
            var repository = new SqlOrderDetailRepository(_contextMock.Object);

            var result = repository.Get();

            Assert.AreEqual(result.Count(), 2);
        }

        [Test]
        public void Get_WhenPredicateOrderDetailId_ReturnOneOrderDetail()
        {
            SetMockDbSet();

            _contextMock.Setup(x => x.Set<OrderDetail>()).Returns(_entitiesMock.Object);
            var repository = new SqlOrderDetailRepository(_contextMock.Object);

            var result = repository.Get(x => x.Id == 1).FirstOrDefault();

            Assert.AreEqual(result.Price, 100);
        }

        private void SetMockDbSet()
        {
            var orderDetails = new List<OrderDetail>
            {
                new OrderDetail() { Id = 1, Price = 100 },
                new OrderDetail() { Id = 2, Price = 200 }
            }.AsQueryable();

            _entitiesMock.As<IQueryable<OrderDetail>>().Setup(m => m.Provider).Returns(orderDetails.Provider);
            _entitiesMock.As<IQueryable<OrderDetail>>().Setup(m => m.Expression).Returns(orderDetails.Expression);
            _entitiesMock.As<IQueryable<OrderDetail>>().Setup(m => m.ElementType).Returns(orderDetails.ElementType);
            _entitiesMock.As<IQueryable<OrderDetail>>().Setup(m => m.GetEnumerator()).Returns(orderDetails.GetEnumerator());
        }
    }
}
