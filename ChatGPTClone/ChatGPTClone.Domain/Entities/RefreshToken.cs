using ChatGPTClone.Domain.Common;

namespace ChatGPTClone.Domain.Entities;

public class RefreshToken : EntityBase<int>
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public string CreatedByIp { get; set; }
    public string SecurityStamp { get; set; }
    public DateTime? Revoked { get; set; }
    public string? RevokedByIp { get; set; }
    public Guid AppUserId { get; set; }
}
