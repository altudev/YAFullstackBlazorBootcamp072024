using System;

namespace ChatGPTClone.Application.Common.Models.Identity;

public class IdentityVerifyEmailRequest
{
    public string Email { get; set; }
    public string Token { get; set; }

    public IdentityVerifyEmailRequest(string email, string token)
    {
        Email = email;
        Token = token;
    }
}
