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
    public class GameAdapterTests
    {
        private Mock<IGenericRepository<Game>> _sqlMock;
        private Mock<IAdvancedMongoRepository<Game>> _mongoMock;
        private Mock<ILogging> _loggerMock;

        [Test]
        public void Create_WhenItemNotNull_CallCreateActionFromSqlRepository()
        {
            var adapter = new GameAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            var game = new Game();

            adapter.Create(game);

            _sqlMock.Verify(x => x.Create(game), Times.Once);
        }

        [Test]
        public void Get_Always_CallGetActionFromBothRepositories()
        {
            var adapter = new GameAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            adapter.Get();

            _sqlMock.Verify(x => x.Get(), Times.Once);
            _mongoMock.Verify(x => x.Get(), Times.Once);
        }

        [Test]
        public void Remove_WhenCrossPropertyIsNull_CallRemoveActionFromSqlRepository()
        {
            var adapter = new GameAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            var game = new Game{CrossProperty = null};

            adapter.Remove(game);

            _sqlMock.Verify(x => x.Remove(game), Times.Once);
        }

        [Test]
        public void Remove_WhenCrossPropertyNotNull_CallRemoveActionFromMongoRepository()
        {
            var adapter = new GameAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            var game = new Game { CrossProperty = "M1" };

            adapter.Remove(game);

            _mongoMock.Verify(x => x.Remove(game), Times.Once);
        }

        [Test]
        public void Update_WhenCrossPropertyIsNull_CallUpdateActionFromSqlRepository()
        {
            var adapter = new GameAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            var game = new Game { CrossProperty = null };

            adapter.Update(game);

            _sqlMock.Verify(x => x.Update(game), Times.Once);
        }

        [Test]
        public void Update_WhenCrossPropertyNotNull_CallUpdateActionFromMongoRepository()
        {
            var adapter = new GameAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            var game = new Game { CrossProperty = "M1" };

            adapter.Update(game);

            _mongoMock.Verify(x => x.Update(game), Times.Once);
        }

        [Test]
        public void Get_WhenPredicateNotNull_CallGetActionFromBothRepositories()
        {
            var adapter = new GameAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            adapter.Get(x => x.Id == 1);

            _sqlMock.Verify(m => m.Get(It.IsAny<Func<Game, bool>>(), null), Times.Once);
            _mongoMock.Verify(m => m.Get(It.IsAny<Func<Game, bool>>(), null), Times.Once);
        }

        [Test]
        public void GetCross_WhenCrossPropertyIsEmpty_CallGetActionFromSqlRepository()
        {
            var adapter = new GameAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            adapter.GetCross(1, string.Empty);

            _sqlMock.Verify(m => m.Get(It.IsAny<Func<Game, bool>>(), null), Times.Once);
        }

        [Test]
        public void GetCross_WhenCrossPropertyNotEmptyOrNull_CallGetActionFromMongoRepository()
        {
            var adapter = new GameAdapter(_sqlMock.Object, _mongoMock.Object, _loggerMock.Object);

            adapter.GetCross(1, "1");

            _mongoMock.Verify(m => m.Get(It.IsAny<Func<Game, bool>>(), null), Times.Once);
        }

        [SetUp]
        public void Init()
        {
            _sqlMock = new Mock<IGenericRepository<Game>>();
            _mongoMock = new Mock<IAdvancedMongoRepository<Game>>();
            _loggerMock = new Mock<ILogging>();

            _loggerMock.Setup(x => x.CudDictionary).Returns(new Dictionary<CUDEnum, string>
            {
                {CUDEnum.Update, "Update" },
                {CUDEnum.Delete, "Delete" },
                {CUDEnum.Create, "Create" }
            });

            _sqlMock.Setup(m => m.Get(It.IsAny<Func<Game, bool>>(), null)).Returns(new List<Game>
            {
                new Game()
            });

            _mongoMock.Setup(m => m.Get(It.IsAny<Func<Game, bool>>(), null)).Returns(new List<Game>
            {
                new Game()
            });
        }
    }
}
