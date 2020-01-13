using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Identity;
using GameStore.Domain.Entities;
using GameStore.Domain.Entities.Identity;
using GameStore.WEB.Auth.Concrete;
using GameStore.WEB.Controllers;
using GameStore.WEB.Models;
using GameStore.WEB.Tests.Tools;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GameStore.WEB.Tests.Controllers
{
    [TestFixture]
    public class CommentControllerTests
    {
        private HttpContextManager _contextManager;

        private Mock<ICommentService> _commentMock;
        private Mock<IGameService> _gameMock;
        private Mock<IIdentityService> _identityMock;

        [Test]
        public void GetAllComments_WhenParameterIsNotNull_CallGetActionCommentService()
        {
            var controller = new CommentController(_commentMock.Object, _gameMock.Object, _identityMock.Object);

            controller.GetComments("1");

            _commentMock.Verify(x => x.GetAllCommentsByGame("1"), Times.Once);
        }

        [Test]
        public void GetAllComments_WhenParameterIsViewModelAndId_CallGetActionCommentService()
        {
            var controller = new CommentController(_commentMock.Object, _gameMock.Object, _identityMock.Object);

            var viewModel = new CommentViewModel();

             controller.GetComments("1", viewModel, "1", "1");

            _commentMock.Verify(x => x.GetAllCommentsByGame("1"), Times.Once);
        }

        [Test]
        public void Delete_WhenCommentIdIsValid_CallDeleteActionFromService()
        {
            var controller = new CommentController(_commentMock.Object, _gameMock.Object, _identityMock.Object);

            controller.Delete("1", "1", "");

            _commentMock.Verify(x => x.Delete(1), Times.Once);
        }

        [Test]
        public void Delete_WhenCommentIdIsNotValid_DoNotCallDeleteActionFromService()
        {
            var controller = new CommentController(_commentMock.Object, _gameMock.Object, _identityMock.Object);

            controller.Delete("NotValid", "1", "");

            _commentMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void Ban_WhenParameterIsLogin_ReturnViewModelIsLogin()
        {
            var controller = new CommentController(_commentMock.Object, _gameMock.Object, _identityMock.Object);

            var result = controller.Ban("login");

            Assert.AreEqual(result.Model, "login");
        }

        [Test]
        public void NewComment_Always_ReturnPartialView()
        {
            var controller = new CommentController(_commentMock.Object, _gameMock.Object, _identityMock.Object);
            
            var result = controller.NewComment();

            Assert.AreEqual(result.GetType(), typeof(PartialViewResult));
        }

        [Test]
        public void DeleteModalWindow_Always_ReturnPartialView()
        {
            var controller = new CommentController(_commentMock.Object, _gameMock.Object, _identityMock.Object);

            var result = controller.DeleteModalWindow();

            Assert.AreEqual(result.GetType(), typeof(PartialViewResult));
        }

        [Test]
        public void AddComment_WhenModelIsValid_CallAddActionFromService()
        {
            _contextManager = new HttpContextManager();
            var controller = new CommentController(_commentMock.Object, _gameMock.Object, _identityMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _contextManager.GetMockedHttpContext()
                }
            };

            var commentView = new CommentViewModel
            {
                Body = "Body"
            };

            controller.AddComment("1", commentView, "1");

            _commentMock.Verify(m => m.AddComment(It.Is<Comment>(c => c.Body == "Body")), Times.Once);
        }

        [SetUp]
        public void SetUp()
        {
            _identityMock = new Mock<IIdentityService>();
            _commentMock = new Mock<ICommentService>();
            _gameMock = new Mock<IGameService>();

            _commentMock.Setup(m => m.GetComment(1)).Returns(new Comment()
            {
                Id = 1,
                Name = "Name",
                CrossProperty = "M1",
                IsQuoted = true,
                IsDeleted = false
            });

            _commentMock.Setup(m => m.GetAllCommentsByGame("1")).Returns(new List<Comment>
            {
                new Comment() {Name = "Name1"},
                new Comment() {Name = "Name2"}
            });


            _gameMock.Setup(m => m.GetGame(It.IsAny<string>())).Returns(new Game() { Name = "Name" });

            _gameMock.Setup(m => m.GetGameByInterimProperty(1, "")).Returns(new Game());

            _identityMock.Setup(m => m.GetUser("login")).Returns(new User
            {
                Roles = new List<Role>
                {
                    new Role{Name = "User"}
                }
            });
        }
    }
}
