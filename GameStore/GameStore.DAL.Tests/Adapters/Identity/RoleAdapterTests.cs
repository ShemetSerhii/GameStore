using System;
using GameStore.DAL.Adapters.Identity;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities.Identity;
using Moq;
using NUnit.Framework;

namespace GameStore.DAL.Tests.Adapters.Identity
{
    [TestFixture]
    public class RoleAdapterTests
    {
        private Mock<IGenericRepository<Role>> _roleMock;

        [Test]
        public void Get_Always_CallGetActionFromRepository()
        {
            var adapter = new RoleAdapter(_roleMock.Object);

            adapter.Get();

            _roleMock.Verify(m => m.Get(), Times.Once);
        }

        [Test]
        public void Get_WhenPredicateNotNull_CallGetActionFromRepository()
        {
            var adapter = new RoleAdapter(_roleMock.Object);

            adapter.Get(x => x.Id == 1);

            _roleMock.Verify(m => m.Get(It.IsAny<Func<Role, bool>>(), null), Times.Once);
        }

        [Test]
        public void Create_WhenItemNotNull_CallCreateActionFromRepository()
        {
            var adapter = new RoleAdapter(_roleMock.Object);
            var role = new Role();

            adapter.Create(role);

            _roleMock.Verify(m => m.Create(role), Times.Once);
        }

        [Test]
        public void Update_WhenItemNotNull_CallUpdateActionFromRepository()
        {
            var adapter = new RoleAdapter(_roleMock.Object);
            var role = new Role();

            adapter.Update(role);

            _roleMock.Verify(m => m.Update(role), Times.Once);
        }

        [Test]
        public void Remove_WhemItemNotNull_CallRemoveActionFromRepository()
        {
            var adapter = new RoleAdapter(_roleMock.Object);
            var role = new Role();

            adapter.Remove(role);

            _roleMock.Verify(m => m.Remove(role), Times.Once);
        }

        [SetUp]
        public void Init()
        {
            _roleMock = new Mock<IGenericRepository<Role>>();
        }
    }
}
