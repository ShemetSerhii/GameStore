using GameStore.BLL.Services;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GameStore.BLL.Tests.Services
{
    [TestFixture]
    public class ShipperServiceTests
    {
        private Mock<IUnitOfWork> _mock;

        [Test]
        public void GetAll_Always_CallGetActionFromAdapter()
        {
            var service = new ShipperService(_mock.Object);

            service.GetAll();

            _mock.Verify(m => m.Shippers.Get(), Times.Once);
        }

        [Test]
        public void Get_WhenParameterIsId_CallGetActionFromAdapter()
        {
            var service = new ShipperService(_mock.Object);

            service.Get(1);

            _mock.Verify(m => m.Shippers.Get(It.IsAny<Func<Shipper, bool>>(), null), Times.Once);
        }

        [SetUp]
        public void Init()
        {
            _mock = new Mock<IUnitOfWork>();

            _mock.Setup(m => m.Shippers.Get());

            _mock.Setup(m => m.Shippers.Get(It.IsAny<Func<Shipper, bool>>(), null)).Returns(new List<Shipper>
            {
                new Shipper()
            });
        }
    }
}
