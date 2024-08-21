using ChatGPTClone.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatGPTClone.Infrastructure.Persistence.Configurations;

public class RoleClaimConfiguration:IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        // Primary key
        builder.HasKey(rc => rc.Id);

        // Maps to the AspNetRoleClaims table
        builder.ToTable("RoleClaims");
    }
}