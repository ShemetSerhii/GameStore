using GameStore.DAL.DBContexts.EF;
using GameStore.DAL.DBContexts.EF.Repositories;
using GameStore.DAL.DBContexts.MongoDB.Intefaces;
using GameStore.Domain.Entities;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GameStore.DAL.Tests.DBContexts.EF.Repositories
{
    [TestFixture]
    class GameRepositoryTests
    {
        private const string GameTestName = "TestName";

        private Mock<SqlContext> _contextMock;
        private Mock<IMongoContext> _mongoMock;
        private Mock<DbSet<Game>> _entitiesMock;
        private Mock<DbSet<Publisher>> _publisherMock;

       [SetUp]
        public void Init()
        {
            _contextMock = new Mock<SqlContext>();
            _entitiesMock = new Mock<DbSet<Game>>();
            _publisherMock = new Mock<DbSet<Publisher>>();
            _mongoMock = new Mock<IMongoContext>();
        }

        [Test]
        public void GameRepository_Add_CallsAddActionFromAppDbContext()
        {
            var game = new Game() {Key = "1"};

            _entitiesMock.Setup(x => x.Add(game));
            _contextMock.Setup(x => x.Set<Game>()).Returns(_entitiesMock.Object);

            var repository = new SqlGameRepository(_contextMock.Object, _mongoMock.Object);
            repository.Create(game);

            _entitiesMock.Verify(x => x.Add(game), Times.Once);
        }

        [Test]
        public void Get_WhenPredicateIsNull_ReturnAllGames()
        {
            SetMockDbSet();

            _contextMock.Setup(x => x.Set<Game>()).Returns(_entitiesMock.Object);
            _contextMock.Setup(x => x.Set<Publisher>()).Returns(_publisherMock.Object);
            var repository = new SqlGameRepository(_contextMock.Object, _mongoMock.Object);

            var result = repository.Get();

            Assert.AreEqual(result.Count(), 2);
        }

        //[Test]
        //public void Get_WhenPredicateGameKey_ReturnOneGame()
        //{
        //    SetMockDbSet();

        //    _contextMock.Setup(x => x.Set<Game>()).Returns(_entitiesMock.Object);
        //    _contextMock.Setup(x => x.Set<Publisher>()).Returns(_publisherMock.Object);
        //    var repository = new SqlGameRepository(_contextMock.Object, _mongoMock.Object);

        //    var result = repository.Get(x => x.Key == "1").FirstOrDefault();

        //    Assert.AreEqual(result.Name, GameTestName);
        //}

     
        private void SetMockDbSet()
        {
            var games = new List<Game>
            {
                new Game() {Id = 1, Key = "1", Name = GameTestName, IsDeleted = false, PublisherId = 1},
                new Game() {Key = "2", Name = "Game2", IsDeleted = false, PublisherId = 2}
            }.AsQueryable();

            _entitiesMock.As<IQueryable<Game>>().Setup(m => m.Provider).Returns(games.Provider);
            _entitiesMock.As<IQueryable<Game>>().Setup(m => m.Expression).Returns(games.Expression);
            _entitiesMock.As<IQueryable<Game>>().Setup(m => m.ElementType).Returns(games.ElementType);
            _entitiesMock.As<IQueryable<Game>>().Setup(m => m.GetEnumerator()).Returns(games.GetEnumerator());

            var publishers = new List<Publisher>
            {
                new Publisher() { Id = 1, CompanyName = "Name", IsDeleted = false},
                new Publisher() { Id = 2, CompanyName = "Name2", IsDeleted = false}
            }.AsQueryable();

            _publisherMock.As<IQueryable<Publisher>>().Setup(m => m.Provider).Returns(publishers.Provider);
            _publisherMock.As<IQueryable<Publisher>>().Setup(m => m.Expression).Returns(publishers.Expression);
            _publisherMock.As<IQueryable<Publisher>>().Setup(m => m.ElementType).Returns(publishers.ElementType);
            _publisherMock.As<IQueryable<Publisher>>().Setup(m => m.GetEnumerator()).Returns(publishers.GetEnumerator());
        }
    }
}
