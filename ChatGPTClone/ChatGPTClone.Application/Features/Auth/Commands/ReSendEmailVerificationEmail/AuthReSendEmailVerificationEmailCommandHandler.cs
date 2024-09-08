using System;
using ChatGPTClone.Application.Common.Interfaces;
using ChatGPTClone.Application.Common.Models.Email;
using ChatGPTClone.Application.Common.Models.General;
using ChatGPTClone.Application.Common.Models.Identity;
using MediatR;

namespace ChatGPTClone.Application.Features.Auth.Commands.ReSendEmailVerificationEmail;

public class AuthReSendEmailVerificationEmailCommandHandler : IRequestHandler<AuthReSendEmailVerificationEmailCommand, ResponseDto<string>>
{
    private readonly IIdentityService _identityService;
    private readonly IEmailService _emailService;
    public AuthReSendEmailVerificationEmailCommandHandler(IIdentityService identityService, IEmailService emailService)
    {
        _identityService = identityService;
        _emailService = emailService;
    }

    public async Task<ResponseDto<string>> Handle(AuthReSendEmailVerificationEmailCommand request, CancellationToken cancellationToken)
    {
        var response = await _identityService.CreateEmailTokenAsync(new IdentityCreateEmailTokenRequest(request.Email), cancellationToken);

        var emailVerificationDto = new EmailVerificationDto(request.Email, response.Token);

        await _emailService.EmailVerificationAsync(emailVerificationDto, cancellationToken);

        return new ResponseDto<string>(data: response.Token, message: "Email verification email sent.");
    }


}
