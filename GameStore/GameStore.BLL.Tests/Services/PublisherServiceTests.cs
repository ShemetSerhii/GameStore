using GameStore.BLL.Services;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.BLL.Tests.Services
{
    [TestFixture]
    public class PublisherServiceTests
    {
        private Mock<IUnitOfWork> _mock;

        [Test]
        public void GetAll_Always_ReturnAllGenresFromAdapter()
        {
            var service = new PublisherService(_mock.Object);

            var result = service.GetAll();

            Assert.AreEqual(result.Count(), 2);
        }

        [Test]
        public void GetByName_WhenParameterIsName_CallGetActionWithPredicateFromAdapter()
        {
            var service = new PublisherService(_mock.Object);

            service.GetByName("Name");

            _mock.Verify(x => x.Publishers.Get(It.IsAny<Func<Publisher, bool>>(), null), Times.Once);
        }

        [Test]
        public void GetByCross_WhenParameterIsIdAndCrossProperty_CallGetActionWithPredicateFromAdapter()
        {
            var service = new PublisherService(_mock.Object);

            service.GetByInterimProperty(1, "Name");

            _mock.Verify(x => x.Publishers.GetCross(1, "Name"), Times.Once);
        }

        [Test]
        public void Delete_WhenParametersIsName_CallRemoveActionFromAdapter()
        {
            var service = new PublisherService(_mock.Object);

            service.Delete("Name");

            _mock.Verify(m => m.Publishers.Remove(It.Is<Publisher>(x => x.CompanyName == "Name")), Times.Once);
        }

        [Test]
        public void Update_WhenItemIsNotNull_CallUpdateActionFromAdapter()
        {
            var service = new PublisherService(_mock.Object);

            var publisher = new Publisher() { CompanyName = "Name" };

            service.Update(publisher);

            _mock.Verify(m => m.Publishers.Update(publisher), Times.Once);
        }

        [Test]
        public void Create_WhenItemIsNotNull_CallCreateActionFromAdapter()
        {
            var service = new PublisherService(_mock.Object);

            var publisher = new Publisher() { CompanyName = "Name" };

            service.Create(publisher);

            _mock.Verify(m => m.Publishers.Create(publisher), Times.Once);
        }

        [Test]
        public void PublisherAccess_WhenParameterIsCompanyNameAndValidLogin_ReturnTrue()
        {
            var service = new PublisherService(_mock.Object);

            var result = service.PublisherAccess("Name", "login");

            Assert.IsTrue(result);
        }

        [Test]
        public void PublisherAccess_WhenParameterIsCompanyNameAndInvalidLogin_ReturnFalse()
        {
            var service = new PublisherService(_mock.Object);

            var result = service.PublisherAccess("Name", "InvalidLogin");

            Assert.IsFalse(result);
        }


        [SetUp]
        public void Init()
        {
            _mock = new Mock<IUnitOfWork>();

            _mock.Setup(x => x.Publishers.Get()).Returns(new List<Publisher>
            {
                new Publisher(),
                new Publisher()
            });

            _mock.Setup(x => x.Publishers.Get(It.IsAny<Func<Publisher, bool>>(), null)).Returns(new List<Publisher>
            {
                new Publisher() {CompanyName = "Name", UserLogin = "login"}
            });
        }
    }
}
