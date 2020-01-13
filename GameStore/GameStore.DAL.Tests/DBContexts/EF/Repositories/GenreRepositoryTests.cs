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
    public class GenreRepositoryTests
    {
        private Mock<SqlContext> _contextMock;
        private Mock<DbSet<Genre>> _entitiesMock;
        private Mock<IMongoContext> _mongoMock;
     
        [Test]
        public void Get_WhenPredicateIsNull_ReturnAllGenres()
        {
            SetMockDbSet();

            _contextMock.Setup(x => x.Set<Genre>()).Returns(_entitiesMock.Object);
            var repository = new SqlGenreRepository(_contextMock.Object, _mongoMock.Object);

            var result = repository.Get();

            Assert.AreEqual(result.Count(), 2);
        }

        [Test]
        public void Get_WhenPredicateIsID_ReturnOneGenre()
        {
            SetMockDbSet();

            _contextMock.Setup(x => x.Set<Genre>()).Returns(_entitiesMock.Object);
            var repository = new SqlGenreRepository(_contextMock.Object, _mongoMock.Object);

            var result = repository.Get(x => x.Id == 1);

            Assert.AreEqual(result.Count(), 1);
        }

        [SetUp]
        public void Init()
        {
            _contextMock = new Mock<SqlContext>();
            _entitiesMock = new Mock<DbSet<Genre>>();
            _mongoMock = new Mock<IMongoContext>();
        }

        private void SetMockDbSet()
        {
            var genres = new List<Genre>
            {
                new Genre() {Id = 1, Name = "Genre1"},
                new Genre() {Id = 2, Name = "Genre2"}
            }.AsQueryable();

            _entitiesMock.As<IQueryable<Genre>>().Setup(m => m.Provider).Returns(genres.Provider);
            _entitiesMock.As<IQueryable<Genre>>().Setup(m => m.Expression).Returns(genres.Expression);
            _entitiesMock.As<IQueryable<Genre>>().Setup(m => m.ElementType).Returns(genres.ElementType);
            _entitiesMock.As<IQueryable<Genre>>().Setup(m => m.GetEnumerator()).Returns(genres.GetEnumerator());
        }
    }
}
