using GameStore.BLL.Interfaces.Identity;
using GameStore.Domain.Entities.Identity;
using GameStore.WEB.Auth.Interfaces;
using GameStore.WEB.Controllers.Identity;
using GameStore.WEB.Models.IdentityModel;
using Moq;
using NUnit.Framework;
using System.Web.Mvc;
using GameStore.WEB.Controllers.Tools;

namespace GameStore.WEB.Tests.Controllers.Identity
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Mock<IAuthentication> _authMock;
        private Mock<IIdentityService> _identityMock;

        [Test]
        public void Get_Login_Always_ReturnLoginViewModel()
        {
            var controller = new AccountController(_authMock.Object, _identityMock.Object);

            var result = controller.Login();

            Assert.AreEqual(result.Model.GetType(), typeof(LoginViewModel));
        }

        [Test]
        public void Get_Register_Always_ReturnRegisterViewModel()
        {
            var controller = new AccountController(_authMock.Object, _identityMock.Object);

            var result = controller.Register();

            Assert.AreEqual(result.Model.GetType(), typeof(RegisterViewModel));
        }

        [Test]
        public void Post_Login_WhenLoginAndPasswordIsValid_ReturnRedirectToRouteResult()
        {
            var controller = new AccountController(_authMock.Object, _identityMock.Object);
            var loginModel = new LoginViewModel {Login = "login", Password = "123", IsPersistent = true};

            var result = controller.Login(loginModel);

            Assert.AreEqual(result.GetType(), typeof(RedirectToRouteResult));
        }

        [Test]
        public void Post_Login_WhenLoginAndPasswordInValid_ReturnViewResult()
        {
            var controller = new AccountController(_authMock.Object, _identityMock.Object);
            var loginModel = new LoginViewModel { Login = "login", Password = "123321", IsPersistent = true };

            var result = controller.Login(loginModel);

            Assert.AreEqual(result.GetType(), typeof(ViewResult));
        }

        [Test]
        public void BanComments_WhenParametersIsLoginAndBanOption_CallUpdateActionFromIdentityService()
        {
            var controller = new AccountController(_authMock.Object, _identityMock.Object);

            controller.BanComments("login", BanOptions.OneDay);

            _identityMock.Verify(m => m.Update(It.Is<User>(u => u.Login == "login")), Times.Once);
        }

        [Test]
        public void Post_Register_WhenLoginAndCompanyNameAlreadyExist_ReturnModelStateIsInvalid()
        {
            var controller = new AccountController(_authMock.Object, _identityMock.Object);
            var resisterModel = new RegisterViewModel
            {
                Name = "Name",
                Login = "login",
                Address = "Kharkiv",
                Password = "12345",
                ConfirmPassword = "12345",
                IsPublisher = true,
                CompanyName = "Company"
            };

            controller.Register(resisterModel);

            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [Test]
        public void Post_Register_WhenCompanyNameLessThreeCharacters_ReturnModelStateIsInvalid()
        {
            var controller = new AccountController(_authMock.Object, _identityMock.Object);
            var resisterModel = new RegisterViewModel
            {
                Name = "Name",
                Login = "login",
                Address = "Kharkiv",
                Password = "12345",
                ConfirmPassword = "12345",
                IsPublisher = true,
                CompanyName = "Co"
            };

            controller.Register(resisterModel);

            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [Test]
        public void Post_Register_WhenCompanyNameIsEmptyString_ReturnModelStateIsInvalid()
        {
            var controller = new AccountController(_authMock.Object, _identityMock.Object);
            var resisterModel = new RegisterViewModel
            {
                Name = "Name",
                Login = "login",
                Address = "Kharkiv",
                Password = "12345",
                ConfirmPassword = "12345",
                IsPublisher = true,
                CompanyName = ""
            };

            controller.Register(resisterModel);

            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [Test]
        public void Post_Register_WhenLoginAndCompanyNameIsValid_CallRegisterActionFromIdentityService()
        {
            var controller = new AccountController(_authMock.Object, _identityMock.Object);
            var resisterModel = new RegisterViewModel
            {
                Name = "Name",
                Login = "Admin",
                Address = "Kharkiv",
                Password = "12345",
                ConfirmPassword = "12345",
                IsPublisher = true,
                CompanyName = "Microsoft"
            };

            controller.Register(resisterModel);

            _identityMock.Verify(m => m.Register(It.Is<User>(u => u.Name == "Name")), Times.Once);
        }

        [SetUp]
        public void Init()
        {
            _authMock = new Mock<IAuthentication>();
            _identityMock = new Mock<IIdentityService>();

            _authMock.Setup(m => m.Login("login", "123", true)).Returns(new User());

            _identityMock.Setup(m => m.GetUser("login")).Returns(new User {Login = "login"});

            _identityMock.Setup(m => m.CompanyNameValidation("Company")).Returns(true);
        }
    }
}
