using GameStore.BLL.Interfaces;
using GameStore.Domain.Entities;
using GameStore.WEB.Controllers;
using GameStore.WEB.Models;
using GameStore.WEB.Tests.Tools;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.WEB.AutoMapper;

namespace GameStore.WEB.Tests.Controllers
{
    [TestFixture]
    public class PublisherControllerTests
    {
        private HttpContextManager _contextManager;
        private Mock<IPublisherService> _publisherMock;

        [Test]
        public void Create_WhenParameterIsEmpty_ReturnViewResult()
        {
            var controller = new PublisherController(_publisherMock.Object);

            var result = controller.Create();

            Assert.AreEqual(result.GetType(), typeof(ViewResult));
        }

        [Test]
        public void Create_WhenParameterIsViewModel_CallCreateActionFromPublisherService()
        {
            var controller = new PublisherController(_publisherMock.Object);
            var publisherView = new PublisherViewModel{CompanyName = "UniqueName"};

            controller.Create(publisherView);

            _publisherMock.Verify(m => m.Create(It.Is<Publisher>(x => x.CompanyName == "UniqueName")), Times.Once);
        }

        [Test]
        public void Create_WhenCompanyNameIsAlreadyExists_DoNotCallCreateActionFromPublisherService()
        {
            var controller = new PublisherController(_publisherMock.Object);
            var publisherView = new PublisherViewModel() { CompanyName = "Name" };

            controller.Create(publisherView);

            _publisherMock.Verify(m => m.Create(It.Is<Publisher>(x => x.CompanyName == "Name")), Times.Never);
        }

        [Test]
        public void Delete_WhenParameterIsId_CallDeleteActionFromPublisherService()
        {
            var controller = new PublisherController(_publisherMock.Object);

            controller.Delete("Name");

            _publisherMock.Verify(m => m.Delete("Name"), Times.Once);
        }

        [Test]
        public void Details_WhenParameterIsCompanyName_CallGetActionFromPublisherService()
        {
            var controller = new PublisherController(_publisherMock.Object);

            controller.Details("1");

            _publisherMock.Verify(m => m.GetByName("1"), Times.Once);
        }

        [Test]
        public void Index_WhenAlways_CallGetAllActionFromPublisherService()
        {
            var controller = new PublisherController(_publisherMock.Object);

            controller.Index();

            _publisherMock.Verify(m => m.GetAll(), Times.Once);
        }

        [Test]
        public void Edit_WhenParameterIsCompanyNameAndPublisherAccessReturnTrue_ReturnViewResult()
        {
            _contextManager = new HttpContextManager();
            var controller = new PublisherController(_publisherMock.Object)
            {
                ControllerContext = new ControllerContext 
                {
                    HttpContext = _contextManager.GetMockedHttpContext()
                }
            };

            var result = controller.Edit("Name");

            Assert.AreEqual(result.GetType(), typeof(ViewResult));
        }

        [Test]
        public void Edit_WhenParameterIsViewModel_CallGetByNameAllActionFromPublisherService()
        {
            var controller = new PublisherController(_publisherMock.Object);
            var model = new PublisherViewModel() { Id = 1, CompanyName = "Name" };

            controller.Edit(model);

            _publisherMock.Verify(m => m.Update(It.Is<Publisher>(x => x.CompanyName == "Name")), Times.Once);
        }

        [Test]
        public void Edit_WhenCompanyNameAlreadyExists_ReturnModelStateIsNotValid()
        {
            var controller = new PublisherController(_publisherMock.Object);
            var model = new PublisherViewModel() { Id = 2, CompanyName = "Name" };

            controller.Edit(model);

            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [SetUp]
        public void SetUp()
        {
            _publisherMock = new Mock<IPublisherService>();

            _publisherMock.Setup(m => m.PublisherAccess("Name", "User")).Returns(true);

            _publisherMock.Setup(m => m.PublisherAccess("1", "1")).Returns(true);

            _publisherMock.Setup(m => m.GetAll()).Returns(new List<Publisher>
            {
                new Publisher
                {
                    Id = 1,
                    CompanyName = "Name",
                    Description = "Info",
                    Games = new List<Game>(),
                    HomePage = "Text"
                }
            });

            _publisherMock.Setup(m => m.GetByInterimProperty(1, null)).Returns(new Publisher
            {
                Id = 1,
                CompanyName = "Name",
                Description = "Info",
                Games = new List<Game>(),
                HomePage = "Text"
            });

            _publisherMock.Setup(m => m.GetByName("Name")).Returns(new Publisher
            {
                Id = 1,
                CompanyName = "Name",
            });
        }
    }
}
