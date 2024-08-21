using ChatGPTClone.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatGPTClone.Infrastructure.Persistence.Configurations;

public class UserRoleConfiguration:IEntityTypeConfiguration<AppUserRole>
{
    public void Configure(EntityTypeBuilder<AppUserRole> builder)
    {
        // Primary key
        builder.HasKey(r => new { r.UserId, r.RoleId });

        // Maps to the AspNetUserRoles table
        builder.ToTable("UserRoles");
    }
}