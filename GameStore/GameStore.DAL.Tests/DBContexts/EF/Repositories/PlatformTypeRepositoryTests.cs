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
    public class PlatformTypeRepositoryTests
    {
        private Mock<SqlContext> _contextMock;
        private Mock<DbSet<PlatformType>> _entitiesMock;

        [SetUp]
        public void Init()
        {
            _contextMock = new Mock<SqlContext>();
            _entitiesMock = new Mock<DbSet<PlatformType>>();
        }

        [Test]
        public void PlatformTypeRepository_Add_CallsAddActionFromAppDbContext()
        {
            var platformType = new PlatformType() { Id = 1 };

            _entitiesMock.Setup(x => x.Add(platformType));
            _contextMock.Setup(x => x.Set<PlatformType>()).Returns(_entitiesMock.Object);

            var repository = new SqlPlatformTypeRepository(_contextMock.Object);
            repository.Create(platformType);

            _entitiesMock.Verify(x => x.Add(platformType), Times.Once);
        }

        [Test]
        public void Get_WhenPredicateIsNull_ReturnAllPlatformTypes()
        {
            SetMockDbSet();

            _contextMock.Setup(x => x.Set<PlatformType>()).Returns(_entitiesMock.Object);
            var repository = new SqlPlatformTypeRepository(_contextMock.Object);

            var result = repository.Get();

            Assert.AreEqual(result.Count(), 2);
        }

        [Test]
        public void Get_WhenPredicateIsID_ReturnOnePlatformType()
        {
            SetMockDbSet();

            _contextMock.Setup(x => x.Set<PlatformType>()).Returns(_entitiesMock.Object);
            var repository = new SqlPlatformTypeRepository(_contextMock.Object);

            var result = repository.Get(x => x.Id == 1);

            Assert.AreEqual(result.Count(), 1);
        }

        private void SetMockDbSet()
        {
            var platformTypes = new List<PlatformType>
            {
                new PlatformType() {Id = 1, Type = "Type1"},
                new PlatformType() {Id = 2, Type = "Type2"}
            }.AsQueryable();

            _entitiesMock.As<IQueryable<PlatformType>>().Setup(m => m.Provider).Returns(platformTypes.Provider);
            _entitiesMock.As<IQueryable<PlatformType>>().Setup(m => m.Expression).Returns(platformTypes.Expression);
            _entitiesMock.As<IQueryable<PlatformType>>().Setup(m => m.ElementType).Returns(platformTypes.ElementType);
            _entitiesMock.As<IQueryable<PlatformType>>().Setup(m => m.GetEnumerator()).Returns(platformTypes.GetEnumerator());
        }
    }
}
