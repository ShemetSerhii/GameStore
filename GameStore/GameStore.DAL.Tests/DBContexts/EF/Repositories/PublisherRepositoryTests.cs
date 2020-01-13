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
    class PublisherRepositoryTests
    {
        private const string PublisherTestName = "TestName";

        private Mock<SqlContext> _contextMock;
        private Mock<DbSet<Publisher>> _entitiesMock;

        [SetUp]
        public void Init()
        {
            _contextMock = new Mock<SqlContext>();
            _entitiesMock = new Mock<DbSet<Publisher>>();

            SetMockDbSet();
        }

        [Test]
        public void PublisherRepository_Add_CallsAddActionFromAppDbContext()
        {
            var publisher = new Publisher() { Id = 1 };

            _entitiesMock.Setup(x => x.Add(publisher));
            _contextMock.Setup(x => x.Set<Publisher>()).Returns(_entitiesMock.Object);

            var repository = new SqlPublisherRepository(_contextMock.Object);
            repository.Create(publisher);

            _entitiesMock.Verify(x => x.Add(publisher), Times.Once);
        }

        [Test]
        public void Get_WhenPredicateIsNull_ReturnAllPublishers()
        {
            _contextMock.Setup(x => x.Set<Publisher>()).Returns(_entitiesMock.Object);
            var repository = new SqlPublisherRepository(_contextMock.Object);

            var result = repository.Get();

            Assert.AreEqual(result.Count(), 2);
        }

        private void SetMockDbSet()
        {
            var publishers = new List<Publisher>
            {
                new Publisher()
                {
                    Id = 1,
                    CompanyName = PublisherTestName,
                    Games = new List<Game>
                    {
                        new Game{IsDeleted = true},
                        new Game{IsDeleted = false}
                    }
                },
                new Publisher() { Id = 2, CompanyName = "Name2"}
            }.AsQueryable();

            _entitiesMock.As<IQueryable<Publisher>>().Setup(m => m.Provider).Returns(publishers.Provider);
            _entitiesMock.As<IQueryable<Publisher>>().Setup(m => m.Expression).Returns(publishers.Expression);
            _entitiesMock.As<IQueryable<Publisher>>().Setup(m => m.ElementType).Returns(publishers.ElementType);
            _entitiesMock.As<IQueryable<Publisher>>().Setup(m => m.GetEnumerator()).Returns(publishers.GetEnumerator());
        }
    }
}
