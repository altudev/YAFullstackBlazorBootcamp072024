using System.Text.Json.Serialization;
using ChatGPTClone.Application.Common.Models.Identity;

namespace ChatGPTClone.Application.Features.Auth.Commands.Login
{
    public class AuthLoginDto
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }

        public AuthLoginDto()
        {

        }

        public AuthLoginDto(string token, DateTime expiresAt, string refreshToken, DateTime refreshTokenExpiresAt)
        {
            Token = token;
            ExpiresAt = expiresAt;
            RefreshToken = refreshToken;
            RefreshTokenExpiresAt = refreshTokenExpiresAt;
        }

        public static AuthLoginDto FromIdentityLoginResponse(IdentityLoginResponse response)
        {
            return new AuthLoginDto(response.Token, response.ExpiresAt, response.RefreshToken, response.RefreshTokenExpiresAt);
        }
    }
}
