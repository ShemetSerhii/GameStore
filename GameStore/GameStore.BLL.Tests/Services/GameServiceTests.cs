using GameStore.BLL.Services;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.BLL.Tests.Services
{
    [TestFixture]
    public class GameServiceTests
    {
        private Mock<IUnitOfWork> _mock;

        [Test]
        public void GetAllGames_CallRepositoryMethod_ReturnListGames()
        {
            var service = new GameService(_mock.Object);

            var result = service.GetAllGames().ToList();

            Assert.AreEqual(result.GetType(), typeof(List<Game>));
        }

        [Test]
        public void GetGenres_WhenAlways_ReturnListGenres()
        {
            var service = new GameService(_mock.Object);

            var result = service.GetGenres().ToList();

            Assert.AreEqual(result.GetType(), typeof(List<Genre>));
        }

        [Test]
        public void GetPlatformTypes_WhenAlways_ReturnListPlatformTypes()
        {
            var service = new GameService(_mock.Object);

            var result = service.GetPlatformTypes().ToList();

            Assert.AreEqual(result.GetType(), typeof(List<PlatformType>));
        }

        [Test]
        public void GetPublishers_WhenAlways_ReturnListPublishers()
        {
            var service = new GameService(_mock.Object);

            var result = service.GetPublishers().ToList();

            Assert.AreEqual(result.GetType(), typeof(List<Publisher>));
        }

        [Test]
        public void DeleteGame_WhenKeyMatch_DeletеGame()
        {
            var service = new GameService(_mock.Object);

            service.DeleteGame("1");

            _mock.Verify(m => m.Games.Remove(It.IsAny<Game>()), Times.Once);
        }

        [Test]
        public void CreateGame_WhenArgumentNotNull_CreateGame()
        {
            var service = new GameService(_mock.Object);
            var game = new Game()
            {
                Id = 1,
                Comments = new List<Comment>(),
                PlatformTypes = new List<PlatformType>(),
                Genres = new List<Genre>()
            };

            service.CreateGame(game);

            _mock.Verify(m => m.Games.Create(game), Times.Once);
        }

        [Test]
        public void UpdateGame_WhenArgumentNotNull_UpdateGame()
        {
            var service = new GameService(_mock.Object);
            var game = new Game();

            service.UpdateGame(game);

            _mock.Verify(m => m.Games.Update(game), Times.Once);
        }

        [Test]
        public void GetGameByCross_WhenArgumentIsNull_ReturnNull()
        {
            var service = new GameService(_mock.Object);

            service.GetGameByInterimProperty(1, "1");

            _mock.Verify(m => m.Games.GetCross(1, "1"), Times.Once);
        }

        [Test]
        public void Pagination_WhenPageSizeIsTwo_ReturnTwoGames()
        {
            var service = new GameService(_mock.Object);

            var games = service.GetAllGames();

            var result = service.Pagination(games, 1, 2);

            Assert.AreEqual(result.Count(), 2);
        }

        [SetUp]
        public void Init()
        {
            _mock = new Mock<IUnitOfWork>();

            _mock.Setup(m => m.Games.Remove(It.IsAny<Game>()));

            _mock.Setup(m => m.Games.Get()).Returns(new List<Game>()
            {
                new Game()
                {
                    Id = 1,
                    Key = "1",
                    Name = "Game",
                    Description = "Info",
                    Price = 100,
                    Discontinued = true,
                    UnitsInStock = 5,
                    PublisherId = 1,
                    Publisher = new Publisher(),
                    IsDeleted = false,
                    Comments = new List<Comment>(),
                    Genres = new List<Genre>()
                    {
                        new Genre()
                        {
                            Id = 1,
                            Name = "Name",
                            ParentId = null,
                            ChildrenGenres = new List<Genre>(),
                            Parent = null,
                            Games = new List<Game>()
                        }
                    },
                    PlatformTypes = new List<PlatformType>()
                    {
                        new PlatformType()
                        {
                            Id = 1,
                            Type = "Type",
                            Games = new List<Game>()
                        }
                    }
                },
                new Game(),
                new Game(),
                new Game()
            });

            _mock.Setup(m => m.Genres.Get()).Returns(new List<Genre>()
            {
                new Genre()
            });

            _mock.Setup(m => m.PlatformTypes.Get()).Returns(new List<PlatformType>()
            {
                new PlatformType()
            });

            _mock.Setup(m => m.Publishers.Get()).Returns(new List<Publisher>()
            {
                new Publisher()
                {
                    Description = "Test",
                    HomePage = "Test",
                }
            });
        }
    }
}
