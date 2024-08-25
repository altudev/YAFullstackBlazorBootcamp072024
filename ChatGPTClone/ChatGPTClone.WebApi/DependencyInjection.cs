using ChatGPTClone.Application.Common.Interfaces;
using ChatGPTClone.WebApi.Services;

namespace ChatGPTClone.WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUserService, CurrentUserManager>();

            return services;
        }
    }
}
