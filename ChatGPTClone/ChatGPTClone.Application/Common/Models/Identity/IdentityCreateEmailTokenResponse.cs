using System;

namespace ChatGPTClone.Application.Common.Models.Identity;

public class IdentityCreateEmailTokenResponse
{
    public string Token { get; set; }

    public IdentityCreateEmailTokenResponse(string token)
    {
        Token = token;
    }
}
