using System;
using ChatGPTClone.Application.Common.Models.Identity;

namespace ChatGPTClone.Application.Features.Auth.Commands.Register;

public class AuthRegisterDto
{
    public Guid UserId { get; set; }

    public string EmailToken { get; set; }


    public AuthRegisterDto(Guid userId, string emailToken)
    {
        UserId = userId;
        EmailToken = emailToken;
    }

    public static AuthRegisterDto Create(IdentityRegisterResponse response)
    {
        return new AuthRegisterDto(response.Id, response.EmailToken);
    }
}
