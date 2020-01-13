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
using System.Linq;

namespace GameStore.DAL.Tests.Adapters
{
    [TestFixture]
    public class GenreAdapterTests
    {
        private Mock<IGenericRepository<Genre>> _sqlMock;
        private Mock<IAdvancedMongoRepository<Genre>> _mongoMock;
        private Mock<ILogging> _loggerMock;

        [Test]
        public void Remove_WhenPublisherHasCrossProperty_CallRemoveFromMongoRepository()
        {
            var adapter = new GenreAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            var genre = new Genre()
            {
                Id = 1,
                Name = "Name",
                CrossProperty = "M",
                IsDeleted = false
            };

            adapter.Remove(genre);

            _mongoMock.Verify(x => x.Remove(genre), Times.Once);
        }

        [Test]
        public void Remove_WhenPublisherNotHasCrossProperty_CallRemoveFromSqlRepository()
        {
            var adapter = new GenreAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            var genre = new Genre()
            {
                Id = 1,
                Name = "Name",
            };

            adapter.Remove(genre);

            _sqlMock.Verify(x => x.Remove(genre), Times.Once);
        }

        [Test]
        public void Crete_WhenItemNotNull_CallCreateActionFromSqlRepository()
        {
            var adapter = new GenreAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            var genre = new Genre { Id = 1 };

            adapter.Create(genre);

            _sqlMock.Verify(x => x.Create(It.Is<Genre>(g => g.Id == 1)), Times.Once);
        }

        [Test]
        public void Get_WhenPredicateNotNull_CallGetActionWithPredicateFromBothRepositories()
        {
            var adapter = new GenreAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            adapter.Get(It.IsAny<Func<Genre, bool>>(), It.IsAny<Func<IEnumerable<Genre>, IOrderedEnumerable<Genre>>>());

            _sqlMock.Verify(x => x.Get(It.IsAny<Func<Genre, bool>>(), It.IsAny<Func<IEnumerable<Genre>, IOrderedEnumerable<Genre>>>()), Times.Once);
            _mongoMock.Verify(x => x.Get(It.IsAny<Func<Genre, bool>>(), It.IsAny<Func<IEnumerable<Genre>, IOrderedEnumerable<Genre>>>()), Times.Once);
        }

        [Test]
        public void Get_Always_CallGetActionFromBothRepositories()
        {
            var adapter = new GenreAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            adapter.Get();

            _sqlMock.Verify(x => x.Get(), Times.Once);
            _mongoMock.Verify(x => x.Get(), Times.Once);
        }

        [Test]
        public void GetCross_WhenCrossPropertyIsNotNull_CallGetActionFromMongoRepository()
        {
            var adapter = new GenreAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            adapter.GetCross(1, "M");

            _mongoMock.Verify(x => x.Get(It.IsAny<Func<Genre, bool>>(), null), Times.Once);
        }

        [Test]
        public void GetCross_WhenCrossProperty_CallGetActionFromSqlRepository()
        {
            var adapter = new GenreAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            adapter.GetCross(1, null);

            _sqlMock.Verify(x => x.Get(It.IsAny<Func<Genre, bool>>(), null), Times.Once);
        }

        [Test]
        public void Update_WhenCrossPropertyIsNull_CallUpdateActionFromSqlRepository()
        {
            var adapter = new GenreAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            var item = new Genre { Id = 1 };

            adapter.Update(item);

            _sqlMock.Verify(x => x.Update(It.Is<Genre>(g => g.Id == 1)), Times.Once);
        }

        [Test]
        public void Update_WhenCrossPropertyIsNotNull_CallUpdateActionFromMongoRepository()
        {
            var adapter = new GenreAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            var item = new Genre { Id = 1, CrossProperty = "M" };

            adapter.Update(item);

            _mongoMock.Verify(x => x.Update(It.Is<Genre>(g => g.Id == 1)), Times.Once);
        }

        [SetUp]
        public void Init()
        {
            _sqlMock = new Mock<IGenericRepository<Genre>>();
            _mongoMock = new Mock<IAdvancedMongoRepository<Genre>>();
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
