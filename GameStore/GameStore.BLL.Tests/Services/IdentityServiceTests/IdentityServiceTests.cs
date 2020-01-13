using GameStore.BLL.Services.IdentityService;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using GameStore.Domain.Entities;

namespace GameStore.BLL.Tests.Services.IdentityServiceTests
{
    [TestFixture]
    public class IdentityServiceTests
    {
        private Mock<IUnitOfWork> _mock;

        [Test]
        public void GetAll_Always_CallGetActionFromAdapter()
        {
            var service = new IdentityService(_mock.Object);

            service.GetAll();

            _mock.Verify(m => m.Users.Get(), Times.Once);
        }

        [Test]
        public void GetUser_WhenParameterIsLogin_CallGetActionFromAdapter()
        {
            var service = new IdentityService(_mock.Object);

            service.GetUser("login");

            _mock.Verify(m => m.Users.Get(It.IsAny<Func<User, bool>>(), null), Times.Once);
        }

        [Test]
        public void Login_WhenParameterIsLoginAndValidPassword_ReturnUser()
        {
            var service = new IdentityService(_mock.Object);

            var result = service.Login("login", "123");

            Assert.IsNotNull(result);
        }

        [Test]
        public void Login_WhenParameterIsLoginAndInValidPassword_ReturnNull()
        {
            var service = new IdentityService(_mock.Object);

            var result = service.Login("login", "123456");

            Assert.IsNull(result);
        }

        [Test]
        public void Update_WhenItemNotNull_CallUpdateActionFromAdapter()
        {
            var service = new IdentityService(_mock.Object);
            var user = new User { Id = 1 };

            service.Update(user);

            _mock.Verify(m => m.Users.Update(user), Times.Once);
        }

        [Test]
        public void Ban_WhenParameterIsUser_ChangeBanStatueToTrue()
        {
            var service = new IdentityService(_mock.Object);
            var user = new User { Id = 1 };

            service.Ban(user);

            Assert.IsTrue(user.IsBanned);
        }

        [Test]
        public void IsBanned_WhenParameterIsNotBannedUserLogin_ReturnFalse()
        {
            var service = new IdentityService(_mock.Object);

            var result = service.IsBanned("login");

            Assert.IsFalse(result);
        }

        [Test]
        public void Register_WhenParameterIsUser_CallCreateActionFromAdapter()
        {
            var service = new IdentityService(_mock.Object);
            var user = new User();

            service.Register(user);

            _mock.Verify(m => m.Users.Create(user), Times.Once);
        }

        [Test]
        public void RegisterPublisher_WhenParameterIsUserAndCompanyName_CallCreateActionFromPublisherAdapter()
        {
            var service = new IdentityService(_mock.Object);
            var user = new User { Login = "Login" };

            service.RegisterPublisher(user, "Company");

            _mock.Verify(m => m.Publishers.Create(It.Is<Publisher>(p => p.CompanyName == "Company")), Times.Once);
        }

        [Test]
        public void CompanyNameValidation_WhenParameterCompanyNameWhichAlreadyExist_ReturnTrue()
        {
            var service = new IdentityService(_mock.Object);

            var result = service.CompanyNameValidation("Valid");

            Assert.IsTrue(result);
        }

        [SetUp]
        public void Init()
        {
            _mock = new Mock<IUnitOfWork>();

            _mock.Setup(m => m.Users.Get());

            _mock.Setup(m => m.Publishers.Get(It.IsAny<Func<Publisher, bool>>(), null)).Returns(new List<Publisher>
            {
                new Publisher {CompanyName = "Valid"}
            });

            _mock.Setup(m => m.Users.Get(It.IsAny<Func<User, bool>>(), null)).Returns(new List<User>
            {
                new User
                {
                    Name = "Name",
                    Login = "login",
                    Password = "123".GetHashCode(),
                    Address = "Kharkiv",
                    IsBanned = false,
                    BanExpires = DateTime.MinValue,
                    Roles = new List<Role>
                    {
                        new Role
                        {
                            Id = 1,
                            Name = "Admin",
                            Users = new List<User>()
                        }
                    }
                }
            });

            _mock.Setup(m => m.Roles.Get(It.IsAny<Func<Role, bool>>(), null)).Returns(new List<Role>
            {
                new Role()
            });

            _mock.Setup(m => m.Publishers.Get()).Returns(new List<Publisher>());
        }
    }
}
