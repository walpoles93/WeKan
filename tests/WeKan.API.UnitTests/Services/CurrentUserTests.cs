using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Security.Claims;
using WeKan.API.Services;
using Xunit;

namespace WeKan.API.UnitTests.Services
{
    public class CurrentUserTests
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;

        public CurrentUserTests()
        {
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
        }

        [Fact]
        public void Ctor_IHttpContextAccessorNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CurrentUser(null));
        }

        [Fact]
        public void IsAuthenticated_UserIsNull_ReturnsFalse()
        {
            var httpContext = new DefaultHttpContext { User = null };
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            var currentUser = new CurrentUser(_httpContextAccessor.Object);

            Assert.False(currentUser.IsAuthenticated);
        }

        [Fact]
        public void IsAuthenticated_UserIdentityNotAuthenticated_ReturnsFalse()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity()); // if no provider is set, user not authenticated
            var httpContext = new DefaultHttpContext { User = user };
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            var currentUser = new CurrentUser(_httpContextAccessor.Object);

            Assert.False(currentUser.IsAuthenticated);
        }

        [Fact]
        public void IsAuthenticated_UserIdentityIsAuthenticated_ReturnsTrue()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { }, "TestProvider")); // if provider is set, user is authenticated
            var httpContext = new DefaultHttpContext { User = user };
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            var currentUser = new CurrentUser(_httpContextAccessor.Object);

            Assert.True(currentUser.IsAuthenticated);
        }

        [Fact]
        public void UserId_NameIdentifierClaimExists_ReturnsNameIdentifier()
        {
            var nameIdentifier = "test-name-identifier";
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, nameIdentifier) }, "TestProvider")); // if provider is set, user is authenticated
            var httpContext = new DefaultHttpContext { User = user };
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            var currentUser = new CurrentUser(_httpContextAccessor.Object);

            Assert.Equal(nameIdentifier, currentUser.UserId);
        }
    }
}
