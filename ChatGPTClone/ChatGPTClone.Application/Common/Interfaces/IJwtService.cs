using ChatGPTClone.Application.Common.Models.Jwt;

namespace ChatGPTClone.Application.Common.Interfaces
{
    public interface IJwtService
    {
        JwtGenerateTokenResponse GenerateToken(JwtGenerateTokenRequest request);
    }
}
