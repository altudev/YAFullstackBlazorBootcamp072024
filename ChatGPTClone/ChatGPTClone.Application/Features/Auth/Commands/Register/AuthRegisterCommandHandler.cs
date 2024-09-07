using System;
using ChatGPTClone.Application.Common.Interfaces;
using ChatGPTClone.Application.Common.Models.Email;
using ChatGPTClone.Application.Common.Models.General;
using MediatR;

namespace ChatGPTClone.Application.Features.Auth.Commands.Register;

public class AuthRegisterCommandHandler : IRequestHandler<AuthRegisterCommand, ResponseDto<AuthRegisterDto>>
{
    private readonly IIdentityService _identityService;
    private readonly IEmailService _emailService;

    public AuthRegisterCommandHandler(IIdentityService identityService, IEmailService emailService)
    {
        _identityService = identityService;
        _emailService = emailService;
    }

    public async Task<ResponseDto<AuthRegisterDto>> Handle(AuthRegisterCommand request, CancellationToken cancellationToken)
    {
        var response = await _identityService.RegisterAsync(request.ToIdentityRegisterRequest(), cancellationToken);

        await _emailService.EmailVerificationAsync(new EmailVerificationDto(response.Email, response.EmailToken), cancellationToken);

        return new ResponseDto<AuthRegisterDto>(AuthRegisterDto.Create(response), "User registered successfully");
    }
}
