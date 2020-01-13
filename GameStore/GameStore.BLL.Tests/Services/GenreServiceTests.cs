using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.Services;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using Moq;
using NUnit.Framework;

namespace GameStore.BLL.Tests.Services
{
    [TestFixture]
    public class GenreServiceTests
    {
        private Mock<IUnitOfWork> _mock;

        [Test]
        public void GetAll_Always_ReturnAllGenresFromAdapter()
        {
            var service = new GenreService(_mock.Object);

            var result = service.GetAll();

            Assert.AreEqual(result.Count(), 2);
        }

        [Test]
        public void Get_WhenParametersIsName_CallGetActionFromAdapterWithPredicate()
        {
            var service = new GenreService(_mock.Object);

            service.Get("Name");

            _mock.Verify(m => m.Genres.Get(It.IsAny<Func<Genre, bool>>(), null), Times.Once);
        }

        [Test]
        public void Delete_WhenParametersIsName_CallRemoveActionFromAdapter()
        {
            var service = new GenreService(_mock.Object);

            service.Delete("Name");

            _mock.Verify(m => m.Genres.Remove(It.Is<Genre>(x => x.Name == "Name")), Times.Once);
        }

        [Test]
        public void Update_WhenItemIsNotNull_CallUpdateActionFromAdapter()
        {
            var service = new GenreService(_mock.Object);

            var genre = new Genre{Name = "Name"};

            service.Update(genre);

            _mock.Verify(m => m.Genres.Update(genre), Times.Once);
        }

        [Test]
        public void Create_WhenItemIsNotNull_CallCreateActionFromAdapter()
        {
            var service = new GenreService(_mock.Object);

            var genre = new Genre { Name = "Name" };

            service.Create(genre);

            _mock.Verify(m => m.Genres.Create(genre), Times.Once);
        }

        [Test]
        public void GetGenreByCross_WhenParameterIsId_ReturnOneGenreFromAdapter()
        {
            var service = new GenreService(_mock.Object);

            var result = service.GetGenreByByInterimProperty(1, null);

            Assert.AreEqual(result.Id, 1);
        }

        [SetUp]
        public void Init()
        {
            _mock = new Mock<IUnitOfWork>();

            _mock.Setup(m => m.Genres.Get()).Returns(new List<Genre>
            {
                new Genre(),
                new Genre()
            });

            _mock.Setup(m => m.Genres.Get(It.IsAny<Func<Genre, bool>>(), null)).Returns(new List<Genre>
            {
                new Genre {Name = "Name"}
            });

            _mock.Setup(m => m.Genres.GetCross(1, null)).Returns(new List<Genre>
            {
                new Genre {Id = 1}
            });
        }
    }
}
