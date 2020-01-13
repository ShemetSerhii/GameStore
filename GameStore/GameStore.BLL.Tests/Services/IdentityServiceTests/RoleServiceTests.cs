using GameStore.BLL.Services.IdentityService;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GameStore.BLL.Tests.Services.IdentityServiceTests
{
    [TestFixture]
    public class RoleServiceTests
    {
        private Mock<IUnitOfWork> _mock;

        [Test]
        public void Get()
        {
            var service = new RoleService(_mock.Object);

            service.Get("Admin");

            _mock.Verify(m => m.Roles.Get(It.IsAny<Func<Role, bool>>(), null), Times.Once);
        }

        [Test]
        public void GetAll()
        {
            var service = new RoleService(_mock.Object);

            service.GetAll();

            _mock.Verify(m => m.Roles.Get(), Times.Once);
        }

        [SetUp]
        public void Init()
        {
            _mock = new Mock<IUnitOfWork>();

            _mock.Setup(m => m.Roles.Get(It.IsAny<Func<Role, bool>>(), null)).Returns(new List<Role>
            {
                new Role()
            });
        }
    }
}
