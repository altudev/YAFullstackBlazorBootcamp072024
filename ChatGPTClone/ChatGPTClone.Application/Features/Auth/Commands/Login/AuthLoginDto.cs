using ChatGPTClone.Application.Common.Models.Identity;

namespace ChatGPTClone.Application.Features.Auth.Commands.Login
{
    public class AuthLoginDto
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }

        public AuthLoginDto(string token, DateTime expiresAt)
        {
            Token = token;

            ExpiresAt = expiresAt;
        }

        public static AuthLoginDto FromIdentityLoginResponse(IdentityLoginResponse response)
        {
            return new AuthLoginDto(response.Token, response.ExpiresAt);
        }
    }
}
