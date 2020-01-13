using GameStore.BLL.Interfaces;
using GameStore.Domain.Entities;
using GameStore.WEB.Controllers;
using GameStore.WEB.Models;
using GameStore.WEB.Models.DomainViewModel.EditorModels;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace GameStore.WEB.Tests.Controllers
{
    [TestFixture]
    public class GenreControllerTests
    {
        private Mock<IGenreService> _genreMock;

        [Test]
        public void Index_Always_CallGetActionFromService()
        {
            var controller = new GenreController(_genreMock.Object);

            controller.Index();

            _genreMock.Verify(m => m.GetAll(), Times.Once);
        }

        [Test]
        public void Get_Create_Always_ReturnViewModelTypeIsGenreEditorModel()
        {
            var controller = new GenreController(_genreMock.Object);

            var result = controller.Create();

            Assert.IsTrue(result.Model is GenreEditorModel);
        }

        [Test]
        public void Delete_WhenParameterIsString_CallDeleteActionFromService()
        {
            var controller = new GenreController(_genreMock.Object);

            controller.Delete("Test");

            _genreMock.Verify(x => x.Delete("Test"), Times.Once);
        }

        [Test]
        public void Post_Create_WhenGenreNameAlreadyExist_ModelStateIsNotValid()
        {
            var controller = new GenreController(_genreMock.Object);

            var model = CreateGenreModel("Test");

            controller.Create(model, null);

            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [Test]
        public void Post_Create_WhenGenreNameIsUnique_CallCreateActionFromService()
        {
            var controller = new GenreController(_genreMock.Object);

            var model = CreateGenreModel("NotTest");

            var parent = new[] {""};

            controller.Create(model, parent);

            _genreMock.Verify(x => x.Create(It.Is<Genre>(g => g.Name == "NotTest")), Times.Once);
        }

        [Test]
        public void Post_Create_WhenGenreNameIsUniqueAndGenreHasParent_CallCreateActionFromService()
        {
            var controller = new GenreController(_genreMock.Object);

            var model = CreateGenreModel("NotTest");

            var parent = new[] { "Parent" };

            controller.Create(model, parent);

            _genreMock.Verify(x => x.Create(It.Is<Genre>(g => g.ParentId == 1)), Times.Once);
        }

        [Test]
        public void Get_Edit_WhenParameterIsGenreName_ReturnViewModelWHasNameTest()
        {
            var controller = new GenreController(_genreMock.Object);

            var view = controller.Edit("Test");

            var result = view.Model as GenreEditorModel;

            Assert.AreEqual(result.Genre.Name, "Test");
        }

        [Test]
        public void Post_Edit_WhenGenreNameAlreadyExist_ModelStateIsNotValid()
        {
            var controller = new GenreController(_genreMock.Object);

            var model = CreateGenreModel("Test");

            controller.Edit(model, null);

            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [Test]
        public void Post_Edit_WhenGenreNameIsUnique_CallEditActionFromService()
        {
            var controller = new GenreController(_genreMock.Object);

            var model = CreateGenreModel("NotTest");

            var parent = new[] { "Parent" };

            controller.Edit(model, parent);

            _genreMock.Verify(x => x.Update(It.Is<Genre>(g => g.Name == "NotTest")), Times.Once);
        }

        [SetUp]
        public void SetUp()
        {
            _genreMock = new Mock<IGenreService>();

            _genreMock.Setup(x => x.Get("Test")).Returns(new Genre
            {
                Id = 1,
                Name = "Test",
                Parent = new Genre
                {
                    Name = "GenreParent"
                }
            });

            _genreMock.Setup(x => x.Get("Parent")).Returns(new Genre
            {
                Id = 1,
                Name = "Parent",
            });

            _genreMock.Setup(x => x.GetAll()).Returns(new List<Genre>
            {
                new Genre {Id = 2, Name = "GenreParent"}
            });

            _genreMock.Setup(x => x.GetGenreByByInterimProperty(3, null)).Returns(new Genre
            {
                Id = 3,
                Name = "NotTest",
                Parent = new Genre
                {
                Name = "Parent"
            }
            });
        }

        private GenreEditorModel CreateGenreModel(string name)
        {
            var model = new GenreEditorModel
            {
                Genre = new GenreViewModel
                {
                    Id = 3,
                    Name = name,
                },
            };

            return model;
        }
    }
}
