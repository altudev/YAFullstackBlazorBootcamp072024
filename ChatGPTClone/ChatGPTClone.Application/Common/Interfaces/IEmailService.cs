using ChatGPTClone.Application.Common.Models.Email;

namespace ChatGPTClone.Application.Common.Interfaces;

public interface IEmailService
{
    Task EmailVerificationAsync(EmailVerificationDto emailVerificationDto, CancellationToken cancellationToken);
}