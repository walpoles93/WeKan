using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using WeKan.API.Services;
using WeKan.Application.Common.Exceptions;
using WeKan.Domain.Users;
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
        public void Ctor_IApplicationDbContextNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CurrentUser(null, _httpContextAccessor.Object));
        }

        [Fact]
        public void Ctor_IHttpContextAccessorNull_ThrowsArgumentNullException()
        {
            var dbName = $"{nameof(CurrentUserTests)}_{nameof(Ctor_IApplicationDbContextNull_ThrowsArgumentNullException)}";
            using var context = TestApplicationDbContext.Create(dbName);

            Assert.Throws<ArgumentNullException>(() => new CurrentUser(context, null));
        }

        [Fact]
        public void IsAuthenticated_UserIsNull_ReturnsFalse()
        {
            var dbName = $"{nameof(CurrentUserTests)}_{nameof(IsAuthenticated_UserIdentityNotAuthenticated_ReturnsFalse)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var httpContext = new DefaultHttpContext { User = null };
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            var currentUser = new CurrentUser(context, _httpContextAccessor.Object);

            Assert.False(currentUser.IsAuthenticated);
        }

        [Fact]
        public void IsAuthenticated_UserIdentityNotAuthenticated_ReturnsFalse()
        {
            var dbName = $"{nameof(CurrentUserTests)}_{nameof(IsAuthenticated_UserIdentityNotAuthenticated_ReturnsFalse)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var user = new ClaimsPrincipal(new ClaimsIdentity()); // if no provider is set, user not authenticated
            var httpContext = new DefaultHttpContext { User = user };
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            var currentUser = new CurrentUser(context, _httpContextAccessor.Object);

            Assert.False(currentUser.IsAuthenticated);
        }

        [Fact]
        public void IsAuthenticated_UserIdentityIsAuthenticated_ReturnsTrue()
        {
            var dbName = $"{nameof(CurrentUserTests)}_{nameof(IsAuthenticated_UserIdentityNotAuthenticated_ReturnsFalse)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { }, "TestProvider")); // if provider is set, user is authenticated
            var httpContext = new DefaultHttpContext { User = user };
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            var currentUser = new CurrentUser(context, _httpContextAccessor.Object);

            Assert.True(currentUser.IsAuthenticated);
        }

        [Fact]
        public void NameIdentifier_IsAuthenticatedFalse_ThrowsUnauthorisedApplicationException()
        {
            var dbName = $"{nameof(CurrentUserTests)}_{nameof(IsAuthenticated_UserIdentityNotAuthenticated_ReturnsFalse)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var user = new ClaimsPrincipal(new ClaimsIdentity()); // if no provider is set, user not authenticated
            var httpContext = new DefaultHttpContext { User = user };
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            var currentUser = new CurrentUser(context, _httpContextAccessor.Object);

            string action() => currentUser.NameIdentifier;

            Assert.Throws<UnauthorisedApplicationException>(action);
        }

        [Fact]
        public void NameIdentifier_NameIdentiferClaimNotExists_ThrowsUnauthorisedApplicationException()
        {
            var dbName = $"{nameof(CurrentUserTests)}_{nameof(IsAuthenticated_UserIdentityNotAuthenticated_ReturnsFalse)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { }, "TestProvider")); // if provider is set, user is authenticated
            var httpContext = new DefaultHttpContext { User = user };
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            var currentUser = new CurrentUser(context, _httpContextAccessor.Object);

            string action() => currentUser.NameIdentifier;

            Assert.Throws<UnauthorisedApplicationException>(action);
        }

        [Fact]
        public void NameIdentifier_NameIdentifierClaimExists_ReturnsNameIdentifier()
        {
            var dbName = $"{nameof(CurrentUserTests)}_{nameof(IsAuthenticated_UserIdentityNotAuthenticated_ReturnsFalse)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var nameIdentifier = "test-name-identifier";
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, nameIdentifier) }, "TestProvider")); // if provider is set, user is authenticated
            var httpContext = new DefaultHttpContext { User = user };
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            var currentUser = new CurrentUser(context, _httpContextAccessor.Object);

            Assert.Equal(nameIdentifier, currentUser.NameIdentifier);
        }

        [Fact]
        public async Task GetId_IsAuthenticatedFalse_ThrowsUnauthorisedApplicationException()
        {
            var dbName = $"{nameof(CurrentUserTests)}_{nameof(IsAuthenticated_UserIdentityNotAuthenticated_ReturnsFalse)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var user = new ClaimsPrincipal(new ClaimsIdentity()); // if no provider is set, user not authenticated
            var httpContext = new DefaultHttpContext { User = user };
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            var currentUser = new CurrentUser(context, _httpContextAccessor.Object);

            Task<int> action() => currentUser.GetId(new CancellationToken());

            await Assert.ThrowsAsync<UnauthorisedApplicationException>(action);
        }

        [Fact]
        public async Task GetId_NameIdentiferClaimNotExists_ThrowsUnauthorisedApplicationException()
        {
            var dbName = $"{nameof(CurrentUserTests)}_{nameof(IsAuthenticated_UserIdentityNotAuthenticated_ReturnsFalse)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { }, "TestProvider")); // if provider is set, user is authenticated
            var httpContext = new DefaultHttpContext { User = user };
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            var currentUser = new CurrentUser(context, _httpContextAccessor.Object);

            Task<int> action() => currentUser.GetId(new CancellationToken());

            await Assert.ThrowsAsync<UnauthorisedApplicationException>(action);
        }

        [Fact]
        public async Task GetId_NameIdentifierExistsUserNotExists_ThrowsNotFoundApplicationException()
        {
            var dbName = $"{nameof(CurrentUserTests)}_{nameof(IsAuthenticated_UserIdentityNotAuthenticated_ReturnsFalse)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var nameIdentifier = "test-name-identifier";
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, nameIdentifier) }, "TestProvider")); // if provider is set, user is authenticated
            var httpContext = new DefaultHttpContext { User = user };
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            var currentUser = new CurrentUser(context, _httpContextAccessor.Object);

            Task<int> action() => currentUser.GetId(new CancellationToken());

            await Assert.ThrowsAsync<NotFoundApplicationException>(action);
        }

        [Fact]
        public async Task GetId_NameIdentifierExistsUserExists_ReturnsUserId()
        {
            var dbName = $"{nameof(CurrentUserTests)}_{nameof(IsAuthenticated_UserIdentityNotAuthenticated_ReturnsFalse)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var nameIdentifier = "test-name-identifier";
            var cancellationToken = new CancellationToken();

            context.Users.Add(new User { NameIdentifier = nameIdentifier });
            await context.SaveChangesAsync(cancellationToken);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, nameIdentifier) }, "TestProvider")); // if provider is set, user is authenticated
            var httpContext = new DefaultHttpContext { User = user };
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            var currentUser = new CurrentUser(context, _httpContextAccessor.Object);

            var userId = await currentUser.GetId(cancellationToken);

            Assert.Equal(1, userId);
        }
    }
}
