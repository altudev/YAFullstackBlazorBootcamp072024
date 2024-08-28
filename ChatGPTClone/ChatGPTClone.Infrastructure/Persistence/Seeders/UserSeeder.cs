using System;
using ChatGPTClone.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatGPTClone.Infrastructure.Persistence.Seeders;

public class UserSeeder : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        var initalUserId = Guid.Parse("2798212b-3e5d-4556-8629-a64eb70da4a8");

        var initialUser = new AppUser
        {
            Id = initalUserId,
            UserName = "alper",
            NormalizedUserName = "ALPER",
            Email = "alper@gmail.com",
            NormalizedEmail = "ALPER@GMAIL.COM",
            EmailConfirmed = true,
            CreatedByUserId = initalUserId.ToString(),
            CreatedOn = new DateTimeOffset(new DateTime(2024, 08, 28)),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            SecurityStamp = Guid.NewGuid().ToString(),
            FirstName = "Alper",
            LastName = "Tunga",
            LockoutEnabled = false,
            AccessFailedCount = 0
        };

        initialUser.PasswordHash = new PasswordHasher<AppUser>().HashPassword(initialUser, "123alper123");

        builder.HasData(initialUser);
    }

}
