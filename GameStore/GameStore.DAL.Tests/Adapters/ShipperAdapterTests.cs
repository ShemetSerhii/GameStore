using GameStore.DAL.Adapters;
using GameStore.DAL.DBContexts.MongoDB.Intefaces;
using GameStore.Domain.Entities;
using Moq;
using NUnit.Framework;
using System;

namespace GameStore.DAL.Tests.Adapters
{
    [TestFixture]
    public class ShipperAdapterTests
    {
        private Mock<IAdvancedMongoRepository<Shipper>> _mock;

        [Test]
        public void Get_Always_CallGetActionFromMongoRepository()
        {
            var adapter = new ShipperAdapter(_mock.Object);

            adapter.Get();

            _mock.Verify(m => m.Get(), Times.Once);
        }

        [Test]
        public void Get_WhenPredicateNotNull_CallGetActionFromMongoRepository()
        {
            var adapter = new ShipperAdapter(_mock.Object);

            adapter.Get(x => x.Id == 1);

            _mock.Verify(m => m.Get(It.IsAny<Func<Shipper, bool>>(), null), Times.Once);
        }

        [SetUp]
        public void Init()
        {
            _mock = new Mock<IAdvancedMongoRepository<Shipper>>();
        }
    }
}
