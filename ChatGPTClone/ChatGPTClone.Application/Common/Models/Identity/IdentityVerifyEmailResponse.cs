using System;

namespace ChatGPTClone.Application.Common.Models.Identity;

public class IdentityVerifyEmailResponse
{
    public string Email { get; set; }

    public IdentityVerifyEmailResponse(string email)
    {
        Email = email;
    }
}
