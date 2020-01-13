using System.Collections.Generic;
using GameStore.BLL.Interfaces.Identity;
using GameStore.Domain.Entities.Identity;
using GameStore.WEB.Auth.Concrete;
using Moq;
using NUnit.Framework;

namespace GameStore.WEB.Tests.Auth.Concrete
{
    [TestFixture]
    public class UserProviderTests
    {
        private Mock<IIdentityService> _identityMock;

        [Test]
        public void IsInRole_WhenRoleIsGuestAndProviderNotFindUser_ReturnTrue()
        {
            var provider = new UserProvider("InvalidLogin", _identityMock.Object);

            var result = provider.IsInRole("Guest");

            Assert.IsTrue(result);
        }

        [Test]
        public void IsInRole_WhenRoleIsUserAndProviderNotFindUser_ReturnFalse()
        {
            var provider = new UserProvider("InvalidLogin", _identityMock.Object);

            var result = provider.IsInRole("User");

            Assert.IsFalse(result);
        }

        [Test]
        public void IsInRole_WhenRoleIsUserAndProviderFindUser_ReturnTrue()
        {
            var provider = new UserProvider("Login", _identityMock.Object);

            var result = provider.IsInRole("User");

            Assert.IsTrue(result);
        }

        [SetUp]
        public void Init()
        {
            _identityMock = new Mock<IIdentityService>();

            _identityMock.Setup(m => m.GetUser("Login")).Returns(new User
            {
                Login = "login",
                Roles = new List<Role>
                {
                    new Role{Name = "User"}
                }
            });
        }
    }
}
