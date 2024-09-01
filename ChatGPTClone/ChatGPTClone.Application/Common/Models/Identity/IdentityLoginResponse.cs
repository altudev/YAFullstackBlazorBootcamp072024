namespace ChatGPTClone.Application.Common.Models.Identity
{
    public class IdentityLoginResponse
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }

        public IdentityLoginResponse(string token, DateTime expiresAt)
        {
            Token = token;

            ExpiresAt = expiresAt;
        }
    }
}
