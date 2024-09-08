using System;
using ChatGPTClone.Application.Common.Models.General;
using MediatR;

namespace ChatGPTClone.Application.Features.Auth.Commands.VerifyEmail;

public class AuthVerifyEmailCommand : IRequest<ResponseDto<string>>
{
    public string Email { get; set; }
    public string Token { get; set; }

    public AuthVerifyEmailCommand(string email, string token)
    {
        Email = email;
        Token = token;
    }

}
