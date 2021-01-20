using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using WeKan.Application.Common.Interfaces;

namespace WeKan.API.Services
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User != null
            && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

        public string UserId => _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}
