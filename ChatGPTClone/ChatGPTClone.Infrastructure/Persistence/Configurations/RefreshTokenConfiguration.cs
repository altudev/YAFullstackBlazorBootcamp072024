using System;
using ChatGPTClone.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatGPTClone.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<RefreshToken> builder)
    {
        // Id
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        // Token
        builder.Property(x => x.Token)
            .IsRequired()
            .HasMaxLength(100);

        // Token Index
        builder
            .HasIndex(x => x.Token)
            .IsUnique();

        // AppUserId, Token
        builder.HasIndex(x => new { x.AppUserId, x.Token });

        // Expires
        builder.Property(x => x.Expires)
            .IsRequired();

        // CreatedByIp
        builder.Property(x => x.CreatedByIp)
            .IsRequired()
            .HasMaxLength(40);

        // Revoked
        builder.Property(x => x.Revoked)
            .IsRequired(false);

        // RevokedByIp
        builder.Property(x => x.RevokedByIp)
            .IsRequired(false)
            .HasMaxLength(40);

        // SecurityStamp
        builder.Property(x => x.SecurityStamp)
            .IsRequired()
            .HasMaxLength(50);

        // CreatedOn
        builder.Property(p => p.CreatedOn)
            .IsRequired();

        // CreatedByUserId
        builder.Property(p => p.CreatedByUserId)
            .IsRequired()
            .HasMaxLength(150);

        // ModifiedOn
        builder.Property(p => p.ModifiedOn)
            .IsRequired(false);

        // ModifiedByUserId
        builder.Property(p => p.ModifiedByUserId)
            .IsRequired(false)
            .HasMaxLength(150);

        builder.ToTable("RefreshTokens");

    }
}
