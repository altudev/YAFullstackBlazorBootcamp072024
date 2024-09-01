using ChatGPTClone.Application.Common.Models.Identity;

namespace ChatGPTClone.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> AuthenticateAsync(IdentityAuthenticateRequest request,CancellationToken cancellationToken);
        Task<IdentityLoginResponse> LoginAsync(IdentityLoginRequest request,CancellationToken cancellationToken);
    }
}
