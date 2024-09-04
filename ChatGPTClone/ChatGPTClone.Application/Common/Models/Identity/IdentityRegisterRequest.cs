using System;

namespace ChatGPTClone.Application.Common.Models.Identity;

public class IdentityRegisterRequest
{
    public string Email { get; set; }

    public string Password { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public IdentityRegisterRequest(string email, string password, string? firstName, string? lastName)
    {
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
    }
}
