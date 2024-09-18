using System.Security.Claims;
using ChatGPTClone.Application.Common.Interfaces;

namespace ChatGPTClone.WebApi.Services
{
    public class CurrentUserManager : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId => GetUserId();

        private Guid GetUserId()
        {
            // return Guid.Parse("2798212b-3e5d-4556-8629-a64eb70da4a8");

            var userId = _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirstValue("uid");

            return string.IsNullOrEmpty(userId) ? Guid.Empty : Guid.Parse(userId);
        }
    }
}
