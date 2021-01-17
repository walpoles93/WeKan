using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Exceptions;
using WeKan.Application.Common.Interfaces;

namespace WeKan.API.Services
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext.User != null
            && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

        public string NameIdentifier
        {
            get
            {
                var nameIdentifier = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (IsAuthenticated && nameIdentifier != null) return nameIdentifier;

                throw new UnauthorisedApplicationException("Name identifier not found for current user");
            }
        }

        public async Task<int> GetId(CancellationToken cancellationToken = default)
        {
            var nameIdentifier = NameIdentifier;
            var userId = await _dbContext.Users
                .Where(u => u.NameIdentifier == nameIdentifier)
                .Select(u => u.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (userId == 0) throw new NotFoundApplicationException($"Could not find user with name identifier: {NameIdentifier}");

            return userId;
        }
    }
}
