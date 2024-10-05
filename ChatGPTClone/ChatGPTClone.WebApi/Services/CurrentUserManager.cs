using System.Security.Claims;
using ChatGPTClone.Application.Common.Interfaces;
using ChatGPTClone.Domain.Helpers;

namespace ChatGPTClone.WebApi.Services
{
    public class CurrentUserManager : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;

        public CurrentUserManager(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            _httpContextAccessor = httpContextAccessor;
            _env = env;
        }

        public Guid UserId => GetUserId();

        public string IpAddress => GetIpAddress();

        private Guid GetUserId()
        {
            // return Guid.Parse("2798212b-3e5d-4556-8629-a64eb70da4a8");

            var userId = _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirstValue("uid");

            return string.IsNullOrEmpty(userId) ? Guid.Empty : Guid.Parse(userId);
        }

        private string GetIpAddress()
        {
            if (_env.IsDevelopment())
                IpHelper.GetIpAddress();

            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                return _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
            else
                return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
        }
    }
}
