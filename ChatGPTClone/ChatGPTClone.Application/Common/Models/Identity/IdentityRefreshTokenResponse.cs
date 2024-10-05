
namespace ChatGPTClone.Application.Common.Models.Identity;

public class IdentityRefreshTokenResponse
{
    public string AccessToken { get; set; }
    public DateTime Expires { get; set; }
    public IdentityRefreshTokenResponse(string accessToken, DateTime expires)
    {
        AccessToken = accessToken;
        Expires = expires;
    }
}
