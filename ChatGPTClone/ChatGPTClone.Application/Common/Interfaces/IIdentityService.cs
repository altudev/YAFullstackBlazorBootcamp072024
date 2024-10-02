using ChatGPTClone.Application.Common.Models.Identity;

namespace ChatGPTClone.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> AuthenticateAsync(IdentityAuthenticateRequest request, CancellationToken cancellationToken);

        Task<bool> CheckEmailExistsAsync(string email, CancellationToken cancellationToken);

        Task<bool> CheckIfEmailVerifiedAsync(string email, CancellationToken cancellationToken);

        Task<bool> CheckSecurityStampAsync(Guid userId, string securityStamp, CancellationToken cancellationToken);

        Task<IdentityRegisterResponse> RegisterAsync(IdentityRegisterRequest request, CancellationToken cancellationToken);
        Task<IdentityLoginResponse> LoginAsync(IdentityLoginRequest request, CancellationToken cancellationToken);
        Task<IdentityVerifyEmailResponse> VerifyEmailAsync(IdentityVerifyEmailRequest request, CancellationToken cancellationToken);

        Task<IdentityCreateEmailTokenResponse> CreateEmailTokenAsync(IdentityCreateEmailTokenRequest request, CancellationToken cancellationToken);
    }
}
