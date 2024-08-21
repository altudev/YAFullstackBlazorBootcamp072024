using ChatGPTClone.Domain.Common;
using ChatGPTClone.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ChatGPTClone.Infrastructure.Identity;

public class AppUser:IdentityUser<Guid>, IEntity, ICreatedByEntity, IModifiedByEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public DateTimeOffset CreatedOn { get; set; }
    public string CreatedByUserId { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }
    public string? ModifiedByUserId { get; set; }
}