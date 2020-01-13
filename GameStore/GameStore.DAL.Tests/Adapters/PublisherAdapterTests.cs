using GameStore.DAL.Adapters;
using GameStore.DAL.DBContexts.MongoDB.Intefaces;
using GameStore.DAL.DBContexts.MongoDB.Logging.Interfaces;
using GameStore.DAL.DBContexts.MongoDB.Logging.LogEntity;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GameStore.DAL.Tests.Adapters
{
    [TestFixture]
    public class PublisherAdapterTests
    {
        private Mock<IGenericRepository<Publisher>> _sqlMock;
        private Mock<IAdvancedMongoRepository<Publisher>> _mongoMock;
        private Mock<ILogging> _loggerMock;

        [Test]
        public void Update_WhenPublisherHasCrossProperty_CallUpdateFromMongoRepository()
        {
            var adapter = new PublisherAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            var publisher = new Publisher
            {
                Id = 1,
                CompanyName = "Name",
                CrossProperty = "M"
            };

            adapter.Update(publisher);

            _mongoMock.Verify(x => x.Update(publisher), Times.Once);
        }

        [Test]
        public void Update_WhenPublisherNotHasCrossProperty_CallUpdateFromSqlRepository()
        {
            var adapter = new PublisherAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            var publisher = new Publisher
            {
                Id = 1,
                CompanyName = "Name",
            };

            adapter.Update(publisher);

            _sqlMock.Verify(x => x.Update(publisher), Times.Once);
        }


        [Test]
        public void Remove_WhenPublisherHasCrossProperty_CallRemoveFromMongoRepository()
        {
            var adapter = new PublisherAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            var publisher = new Publisher
            {
                Id = 1,
                CompanyName = "Name",
                CrossProperty = "M"
            };

            adapter.Remove(publisher);

            _mongoMock.Verify(x => x.Remove(publisher), Times.Once);
        }

        [Test]
        public void Remove_WhenPublisherNotHasCrossProperty_CallRemoveFromSqlRepository()
        {
            var adapter = new PublisherAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            var publisher = new Publisher
            {
                Id = 1,
                CompanyName = "Name",
            };

            adapter.Remove(publisher);

            _sqlMock.Verify(x => x.Remove(publisher), Times.Once);
        }

        [Test]
        public void Get_Always_CallGetActionFromBothRepositories()
        {
            var adapter = new PublisherAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            adapter.Get();

            _sqlMock.Verify(x => x.Get(), Times.Once);
            _mongoMock.Verify(x => x.Get(), Times.Once);
        }

        [Test]
        public void Create_WhenItemNotNull_CallCreateActionFromSqlRepository()
        {
            var  adapter = new PublisherAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            var publisher = new Publisher {Id = 1};

            adapter.Create(publisher);

            _sqlMock.Verify(x => x.Create(It.Is<Publisher>(p => p.Id == 1)), Times.Once);
        }

        [Test]
        public void GetCross_WhenCrossPropertyIsNull_CallGetActionFromSqlRepository()
        {
            var adapter = new PublisherAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            adapter.GetCross(1, null);

            _sqlMock.Verify(x => x.Get(It.IsAny<Func<Publisher, bool>>(), null), Times.Once);
        }

        [Test]
        public void GetCross_WhenCrossPropertyNotNull_CallGetActionFromMongoRepository()
        {
            var adapter = new PublisherAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            adapter.GetCross(1, "Cross");

            _mongoMock.Verify(x => x.Get(It.IsAny<Func<Publisher, bool>>(), null), Times.Once);
        }

        [SetUp]
        public void Init()
        {
            _sqlMock = new Mock<IGenericRepository<Publisher>>();
            _mongoMock = new Mock<IAdvancedMongoRepository<Publisher>>();
            _loggerMock = new Mock<ILogging>();

            _loggerMock.Setup(x => x.CudDictionary).Returns(new Dictionary<CUDEnum, string>
            {
                {CUDEnum.Update, "Update" },
                {CUDEnum.Delete, "Delete" },
                {CUDEnum.Create, "Create" }
            });

            _sqlMock.Setup(x => x.Get()).Returns(new List<Publisher>
            {
                new Publisher {CompanyName = "1"}
            });

            _mongoMock.Setup(x => x.Get()).Returns(new List<Publisher>
            {
                new Publisher {CompanyName = "2"}
            });
        }
    }
}
