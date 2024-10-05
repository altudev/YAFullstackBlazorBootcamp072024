namespace ChatGPTClone.Application.Common.Models.Identity;

public class IdentityLoginResponse
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }

    public IdentityLoginResponse(string token, DateTime expiresAt, string refreshToken, DateTime refreshTokenExpiresAt)
    {
        Token = token;
        RefreshToken = refreshToken;
        ExpiresAt = expiresAt;
        RefreshTokenExpiresAt = refreshTokenExpiresAt;
    }
}
