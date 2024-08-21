using ChatGPTClone.Domain.Entities;
using ChatGPTClone.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatGPTClone.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        //Id
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        // Indexes for "normalized" username and email, to allow efficient lookups
        builder.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
        builder.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");

        // A concurrency token for use with the optimistic concurrency checking
        builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

        // Limit the size of columns to use efficient database types
        builder.Property(u => u.UserName).HasMaxLength(100);
        builder.Property(u => u.NormalizedUserName).HasMaxLength(100);
            
        //Email
        builder.Property(u => u.Email).IsRequired();
        builder.HasIndex(user => user.Email).IsUnique();
        builder.Property(u => u.Email).HasMaxLength(100);
        builder.Property(u => u.NormalizedEmail).HasMaxLength(100);

        //PhoneNumber
        builder.Property(u => u.PhoneNumber).IsRequired(false);
        builder.Property(u => u.PhoneNumber).HasMaxLength(20);



        // The relationships between User and other entity types
        // Note that these relationships are configured with no navigation properties

        // Each User can have many UserClaims
        builder.HasMany<AppUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

        // Each User can have many UserLogins
        builder.HasMany<AppUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

        // Each User can have many UserTokens
        builder.HasMany<AppUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

        // Each User can have many entries in the UserRole join table
        builder.HasMany<AppUserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

        // Each User can have many ChatSessions
        builder.HasMany<ChatSession>()
            .WithOne()
            .HasForeignKey(x => x.AppUserId);

        // Common Properties

        // CreatedDate
        builder.Property(x => x.CreatedOn).IsRequired();

        // CreatedByUserId
        builder.Property(user => user.CreatedByUserId)
            .HasMaxLength(100)
            .IsRequired();

        // ModifiedDate
        builder.Property(user => user.ModifiedOn)
            .IsRequired(false);

        // ModifiedByUserId
        builder.Property(user => user.ModifiedByUserId)
            .HasMaxLength(100)
            .IsRequired(false);


        builder.ToTable("Users");
    }
}