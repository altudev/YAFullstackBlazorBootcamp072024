using ChatGPTClone.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatGPTClone.Infrastructure.Persistence.Configurations;

public class UserClaimConfiguration:IEntityTypeConfiguration<AppUserClaim>
{
    public void Configure(EntityTypeBuilder<AppUserClaim> builder)
    {
        // Primary key
        builder.HasKey(x => x.Id);

        // Maps to the AspNetUserClaims table
        builder.ToTable("UserClaims");
    }
}