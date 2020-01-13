using GameStore.DAL.Adapters;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using Moq;
using NUnit.Framework;
using System;

namespace GameStore.DAL.Tests.Adapters
{
    [TestFixture]
    public class PlatformTypeAdapterTests
    {
        private Mock<IGenericRepository<PlatformType>> _sqlMock;

        [Test]
        public void Create_WhenItemNotNull_CallCreateActionFromSqlRepository()
        {
            var adapter = new PlatformTypeAdapter(_sqlMock.Object);

            var platform = new PlatformType();

            adapter.Create(platform);

            _sqlMock.Verify(x => x.Create(platform), Times.Once);
        }

        [Test]
        public void Get_Always_CallGetActionFromSqlRepository()
        {
            var adapter = new PlatformTypeAdapter(_sqlMock.Object);

            adapter.Get();

            _sqlMock.Verify(x => x.Get(), Times.Once);
        }

        [Test]
        public void Get_WhenPredicateNotNull_CallGetActionWithPredicateFromSqlRepository()
        {
            var adapter = new PlatformTypeAdapter(_sqlMock.Object);

            adapter.Get(It.IsAny<Func<PlatformType, bool>>());

            _sqlMock.Verify(x => x.Get(It.IsAny<Func<PlatformType, bool>>(), null), Times.Once);
        }

        [Test]
        public void Update_WhenItemNotNull_CallUpdateActionFromSqlRepository()
        {
            var adapter = new PlatformTypeAdapter(_sqlMock.Object);

            var platform = new PlatformType();

            adapter.Update(platform);

            _sqlMock.Verify(x => x.Update(platform), Times.Once);
        }

        [Test]
        public void Remove_WhenItemNotNull_CallRemoveActionFromSqlRepository()
        {
            var adapter = new PlatformTypeAdapter(_sqlMock.Object);

            var platform = new PlatformType();

            adapter.Remove(platform);

            _sqlMock.Verify(x => x.Remove(platform), Times.Once);
        }

        [SetUp]
        public void Init()
        {
            _sqlMock = new Mock<IGenericRepository<PlatformType>>();
        }
    }
}
