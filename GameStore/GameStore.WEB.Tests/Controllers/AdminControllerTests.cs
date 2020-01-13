using GameStore.BLL.Interfaces.Identity;
using GameStore.Domain.Entities.Identity;
using GameStore.WEB.Controllers;
using GameStore.WEB.Models.DomainViewModel.EditorModels;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace GameStore.WEB.Tests.Controllers
{
    [TestFixture]
    public class AdminControllerTests
    {
        private Mock<IIdentityService> _identityMock;
        private Mock<IRoleService> _roleMock;

        [Test]
        public void Index_Always_CallGetActionFromIdentityService()
        {
            var controller = new AdminController(_identityMock.Object, _roleMock.Object);

            controller.Index();

            _identityMock.Verify(m => m.GetAll(), Times.Once);
        }

        [Test]
        public void UserDetails_WhenParameterIsLogin_CallGetActionFromIdentityService()
        {
            var controller = new AdminController(_identityMock.Object, _roleMock.Object);

            controller.UserDetails("login");

            _identityMock.Verify(m => m.GetUser("login"), Times.Once);
        }

        [Test]
        public void Get_Edit_WhenParameterIsLogin_ReturnUserEditorModel()
        {
            var controller = new AdminController(_identityMock.Object, _roleMock.Object);

            var result = controller.Edit("login");

            Assert.AreEqual(result.Model.GetType(), typeof(UserEditorModel));
        }

        [Test]
        public void Post_Edit_WhenParameterIsUserModel_CallUpdateActionFromIdentityService()
        {
            var controller = new AdminController(_identityMock.Object, _roleMock.Object);
            var userModel = new UserEditorModel { Login = "login" };
            var rolesSelected = new[] {"Admin"};

            controller.Edit(userModel, rolesSelected);

            _identityMock.Verify(m => m.Update(It.Is<User>(u => u.Login == userModel.Login)));
        }

        [SetUp]
        public void SetUp()
        {
            _identityMock = new Mock<IIdentityService>();
            _roleMock = new Mock<IRoleService>();

            _identityMock.Setup(m => m.GetUser("login")).Returns(new User
            {
                Login = "login",
                Roles = new List<Role>
                {
                    new Role{Name = "Admin"},
                    new Role{Name = "Manager"}
                }
            });

            _roleMock.Setup(m => m.GetAll()).Returns(new List<Role>
            {
                new Role {Name = "Admin"},
                new Role {Name = "Manager"}
            });
        }
    }
}
