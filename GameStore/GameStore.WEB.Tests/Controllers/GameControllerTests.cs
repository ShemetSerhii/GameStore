using GameStore.BLL.FilterPipeline.Concrete;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using GameStore.WEB.App_Start;
using GameStore.WEB.Controllers;
using GameStore.WEB.Models;
using GameStore.WEB.Models.FilterModel;
using GameStore.WEB.Tests.Tools;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace GameStore.WEB.Tests.Controllers
{
    [TestFixture]
    public class GameControllerTests
    {
        private HttpContextManager _contextManager;

        private Mock<IGameService> _gameMock;
        private Mock<IPublisherService> _publisherMock;
        private Mock<GameSelectionPipeline> _pipelineMock;
        private Mock<IUnitOfWork> _unitMock;

        [OneTimeSetUp]
        public void Init()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        [Test]
        public void Index_WhenAlways_CallServiceMethod()
        {
            var controller = new GameController(_gameMock.Object, _publisherMock.Object, _pipelineMock.Object);

            controller.Index();

            _gameMock.Verify(m => m.GetAllGames(), Times.AtLeastOnce);
        }
      
        [Test]
        public void Create_WhenParametersIsModelAndKeyIsUnique_CallCreateGameActionFromGameService()
        {
            var controller = new GameController(_gameMock.Object, _publisherMock.Object, _pipelineMock.Object);
            var gameView = new GameViewModel() { Key = "3", DatePublication = DateTime.UtcNow.AddDays(-2).ToString()};

            string[] publishers = new[] { "1", "2" };
            string[] genres = new[] { "1", "2" };
            string[] platformTypes = new[] { "1", "2" };

            controller.New(gameView, publishers, genres, platformTypes);

            _gameMock.Verify(m => m.CreateGame(It.IsAny<Game>()), Times.Once);
        }


        [Test]
        public void Create_WhenPublisherIsNull_MotelStateIsNotValid()
        {
            var controller = new GameController(_gameMock.Object, _publisherMock.Object, _pipelineMock.Object);
            var gameView = new GameViewModel() { Key = "3", DatePublication = DateTime.UtcNow.AddDays(-2).ToString() };

            string[] publishers = null;
            string[] genres = new[] { "1", "2" };
            string[] platformTypes = new[] { "1", "2" };

            controller.New(gameView, publishers, genres, platformTypes);

            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [Test]
        public void Create_WhenGenresIsNull_MotelStateIsNotValid()
        {
            var controller = new GameController(_gameMock.Object, _publisherMock.Object, _pipelineMock.Object);
            var gameView = new GameViewModel() { Key = "3", DatePublication = DateTime.UtcNow.AddDays(-2).ToString() };

            string[] publishers = new[] { "1", "2" };
            string[] genres = null;
            string[] platformTypes = new[] { "1", "2" };

            controller.New(gameView, publishers, genres, platformTypes);

            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [Test]
        public void Create_WhenPlatformTypeIsNull_MotelStateIsNotValid()
        {
            var controller = new GameController(_gameMock.Object, _publisherMock.Object, _pipelineMock.Object);
            var gameView = new GameViewModel() { Key = "3", DatePublication = DateTime.UtcNow.AddDays(-2).ToString() };

            string[] publishers = new[] { "1", "2" };
            string[] genres = new[] { "1", "2" };
            string[] platformTypes = null;

            controller.New(gameView, publishers, genres, platformTypes);

            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [Test]
        public void Create_WhenGameKeyAlreadyExists_MotelStateIsNotValid()
        {
            var controller = new GameController(_gameMock.Object, _publisherMock.Object, _pipelineMock.Object);
            var gameView = new GameViewModel() { Key = "1" , DatePublication = DateTime.UtcNow.ToString()};

            string[] publishers = new[] { "1", "2" };
            string[] genres = new[] { "1", "2" };
            string[] platformTypes = new[] { "1", "2" };

            controller.New(gameView, publishers, genres, platformTypes);

            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [Test]
        public void Get_Edit_WhenEditHasAccess_ReturnViewResult()
        {
            _contextManager = new HttpContextManager();
            var controller = new GameController(_gameMock.Object, _publisherMock.Object, _pipelineMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _contextManager.GetMockedHttpContext()
                }
            };

            var result = controller.Update("5");

            Assert.AreEqual(result.GetType(), typeof(ViewResult));
        }

        [Test]
        public void Get_Edit_WhenEditHasAccess__ReturnViewResult()
        {
            _contextManager = new HttpContextManager();
            var controller = new GameController(_gameMock.Object, _publisherMock.Object, _pipelineMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _contextManager.GetMockedHttpContext()
                }
            };

            var result = controller.Update("1");

            Assert.AreEqual(result.GetType(), typeof(ViewResult));
        }

        [Test]
        public void Post_Edit_WhenParametersIsViewModelAndSelectedItems_UpdateOfTheModelWeCreated()
        {
            var controller = new GameController(_gameMock.Object, _publisherMock.Object, _pipelineMock.Object);
            var gameView = new GameViewModel() {Id = 1, Key = "1", DatePublication = DateTime.UtcNow.AddDays(-2).ToString()};

            string[] publishers = new[] { "1", "2" };
            string[] genres = new[] { "1", "2" };
            string[] platformTypes = new[] { "1", "2" };

            controller.Update(gameView, publishers, genres, platformTypes);

            _gameMock.Verify(m => m.UpdateGame(It.Is<Game>(g => g.Id  == gameView.Id)), Times.Once);
        }

        [Test]
        public void Post_Edit_WhenPublishersIsNull_MotelStateIsNotValid()
        {
            var controller = new GameController(_gameMock.Object, _publisherMock.Object, _pipelineMock.Object);
            var gameView = new GameViewModel() {Key = "1", DatePublication = DateTime.UtcNow.AddDays(-2).ToString() };

            string[] publishers = null;
            string[] genres = new[] { "1", "2" };
            string[] platformTypes = new[] { "1", "2" };

            controller.Update(gameView, publishers, genres, platformTypes);

            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [Test]
        public void Post_Edit_WhenGenresIsNull_MotelStateIsNotValid()
        {
            var controller = new GameController(_gameMock.Object, _publisherMock.Object, _pipelineMock.Object);
            var gameView = new GameViewModel() { Key = "1", DatePublication = DateTime.UtcNow.AddDays(-2).ToString() };

            string[] publishers = new[] { "1", "2" };
            string[] genres = null;
            string[] platformTypes = new[] { "1", "2" };

            controller.Update(gameView, publishers, genres, platformTypes);

            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [Test]
        public void Post_Edit_WhenPlatformTypesIsNull_MotelStateIsNotValid()
        {
            var controller = new GameController(_gameMock.Object, _publisherMock.Object, _pipelineMock.Object);
            var gameView = new GameViewModel() { Key = "1", DatePublication = DateTime.UtcNow.AddDays(-2).ToString() };

            string[] publishers = new[] { "1", "2" };
            string[] genres = new[] { "1", "2" };
            string[] platformTypes = null;

            controller.Update(gameView, publishers, genres, platformTypes);

            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [Test]
        public void Post_Edit_WhenParametersIsViewModelAndNotExistsSelectedItems_ReturnNotValidView()
        {
            var controller = new GameController(_gameMock.Object, _publisherMock.Object, _pipelineMock.Object);
            var gameView = new GameViewModel() { Key = "1", DatePublication = DateTime.UtcNow.AddDays(-2).ToString() };

            string[] publishers = null;
            string[] genres = null;
            string[] platformTypes = null;

            var result = controller.Update(gameView, publishers, genres, platformTypes);

            Assert.AreEqual(result.GetType(), typeof(ViewResult));
        }

        [Test]
        public void Delete_WhenParametersIsKey_CallDeleteActionFromGameService()
        {        
            var controller = new GameController(_gameMock.Object, _publisherMock.Object, _pipelineMock.Object);

            controller.Delete("1");

            _gameMock.Verify(m => m.DeleteGame("1"), Times.Once);
        }

        [Test]
        public void Details_GetDetailsByKey_CallServiceMethod()
        {
            var controller = new GameController(_gameMock.Object, _publisherMock.Object, _pipelineMock.Object);

            controller.Details("1");

            _gameMock.Verify(m => m.GetGame("1"), Times.Once);
        }

        [Test]
        public void Filter_WhenGivenModelForRegistation_CallProcessActionFromFilterPipeline()
        {
            var controller = new GameController(_gameMock.Object, _publisherMock.Object, _pipelineMock.Object);

            var filterModel = new FilterViewModel
            {
                GenresId = new int[] { },
                PlatformTypesId = new int[] { },
                SortingType = SortFilter.MostPopular,
                PartOfName = "Name"
            };

            controller.Filter(filterModel);

            _pipelineMock.Verify(x => x.Process(), Times.Once);
        }

        [SetUp]
        public void SetUp()
        {
            _gameMock = new Mock<IGameService>();
            _publisherMock = new Mock<IPublisherService>();
            _unitMock = new Mock<IUnitOfWork>();
            _pipelineMock = new Mock<GameSelectionPipeline>(_unitMock.Object);


            _gameMock.Setup(m => m.GetGameByInterimProperty(1, null)).Returns(new Game
            {
                Id = 1,
                Key = "1",
                Name = "Game1",
                Comments = new List<Comment>(),
                Genres = new List<Genre>()
                {
                    new Genre() {Id = 1},
                    new Genre() {Id = 2}
                },
                Publisher = new Publisher() { Id = 1 },
                PlatformTypes = new List<PlatformType>()
                {
                    new PlatformType() {Id = 1},
                    new PlatformType() {Id = 2}
                }
            });

            _gameMock.Setup(m => m.GetGame("1")).Returns(new Game
            {
                Id = 1,
                Key = "1",
                Name = "Game1",
                Genres = new List<Genre>()
                {
                    new Genre() {Id = 1},
                    new Genre() {Id = 2}
                },
                Publisher = new Publisher() { Id = 1, CompanyName = "Company"},
                PlatformTypes = new List<PlatformType>()
                {
                    new PlatformType() {Id = 1},
                    new PlatformType() {Id = 2}
                }
            });

            _gameMock.Setup(m => m.GetGame("5")).Returns(new Game
            {
                Id = 2,
                Key = "5",
                Name = "Game1",
                Genres = new List<Genre>()
                {
                    new Genre() {Id = 1},
                    new Genre() {Id = 2}
                },
                PlatformTypes = new List<PlatformType>()
                {
                    new PlatformType() {Id = 1},
                    new PlatformType() {Id = 2}
                }
            });

            _gameMock.Setup(m => m.GetAllGames()).Returns(new List<Game>()
            {
                new Game()
                {
                    Key = "1",
                    Name = "Game1",
                    PlatformTypes = new List<PlatformType>()
                    {
                        new PlatformType() {Id = 1, Type = "Type"}
                    },
                    Genres = new List<Genre>()
                    {
                        new Genre()
                        {
                            Id = 1,
                            Name = "Name",
                            ParentId = null,
                            ChildrenGenres = new List<Genre>()
                        }
                    }
                },
            });

            _gameMock.Setup(m => m.GetGenres()).Returns(new List<Genre>
            {
                new Genre() {Id = 1, Name = "1"},
                new Genre() {Id = 2, Name = "2"}
            });

            _gameMock.Setup(m => m.GetPlatformTypes()).Returns(new List<PlatformType>
            {
                new PlatformType() {Id = 1, Type = "1"},
                new PlatformType() {Id = 2, Type = "2"}
            });

            _gameMock.Setup(m => m.GetPublishers()).Returns(new List<Publisher>
            {
                new Publisher() {Id = 1, CompanyName = "1"},
                new Publisher() {Id = 2, CompanyName = "2"}
            });

            _publisherMock.Setup(m => m.PublisherAccess("Company", "admin")).Returns(true);

            _publisherMock.Setup(m => m.PublisherAccess("Company", "user")).Returns(false);
        }
    }
}
