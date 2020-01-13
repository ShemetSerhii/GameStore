using GameStore.BLL.Interfaces.Identity;
using GameStore.Domain.Entities.Identity;
using GameStore.WEB.Auth.Concrete;
using GameStore.WEB.AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Security.Principal;
using System.Web;

namespace GameStore.WEB.Tests.Auth.Concrete
{
    [TestFixture]
    public class CustomAuthenticationTests
    {
        private Mock<IIdentityService> _identityMock;
        private Mock<IPrincipal> _principalMock;

        [Test]
        public void Login_WhenLoginIsValid_CreateAuthCookie()
        {
            var auth = new CustomAuthentication(_identityMock.Object);
            auth.HttpContext = CreateHttpContext();

            auth.Login("User");
            var result = auth.HttpContext.Response.Cookies.Count;

            Assert.AreEqual(result, 1);
        }

        [Test]
        public void LogOut_Always_ClearAuthCookie()
        {
            var auth = new CustomAuthentication(_identityMock.Object);
            auth.HttpContext = CreateHttpContext();

            auth.LogOut();
            var result = auth.HttpContext.Response.Cookies["__AUTH_COOKIE"].Value;

            Assert.AreEqual(result, string.Empty);
        }

        [Test]
        public void Login_WhenParametersIsLoginAndPassword_CallLoginActionFromService()
        {
            var auth = new CustomAuthentication(_identityMock.Object);
            auth.HttpContext = CreateHttpContext();

            auth.Login("User", "123", true);

            _identityMock.Verify(m => m.Login("User", "123"), Times.Once);
        }

        [SetUp]
        public void Init()
        {
            _identityMock = new Mock<IIdentityService>();
            _principalMock = new Mock<IPrincipal>();

            _identityMock.Setup(m => m.GetUser("User")).Returns(new User
            {
                Login = "User",
            });

            _identityMock.Setup(m => m.Login("User", "123")).Returns(new User
            {
                Login = "User"
            });
        }

        [OneTimeSetUp]
        public void OneInit()
        {
            var configuration = new AutoMapperConfiguration();
            configuration.Config();
        }

        private HttpContext CreateHttpContext()
        {
            return new HttpContext(new HttpRequest("1", new Uri("http://www.google.com").AbsoluteUri, "3"), new HttpResponse(TextWriter.Null));
        }
    }
}
