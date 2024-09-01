namespace ChatGPTClone.Application.Common.Models.Jwt
{
    public class JwtGenerateTokenResponse
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }

        public JwtGenerateTokenResponse(string token, DateTime expiresAt)
        {
            Token = token;

            ExpiresAt = expiresAt;
        }
    }
}
