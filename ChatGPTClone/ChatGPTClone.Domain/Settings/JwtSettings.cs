using System;

namespace ChatGPTClone.Domain.Settings;

public class JwtSettings
{
    public string SecretKey { get; set; }
    public TimeSpan AccessTokenExpiration { get; set; }
    public TimeSpan RefreshTokenExpiration { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
