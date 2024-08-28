using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatGPTClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedingInitialUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedByUserId", "CreatedOn", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "ModifiedByUserId", "ModifiedOn", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("2798212b-3e5d-4556-8629-a64eb70da4a8"), 0, "3341db54-d6a0-4921-813a-355ddfa7b935", "2798212b-3e5d-4556-8629-a64eb70da4a8", new DateTimeOffset(new DateTime(2024, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "alper@gmail.com", true, "Alper", "Tunga", false, null, null, null, "ALPER@GMAIL.COM", "ALPER", "AQAAAAIAAYagAAAAEBmJM5wYkRyxEwI0vrw2aRKBV4XJQWCJFm0/gbyRwYWBHB96loqrt+LgeOYoi38OgQ==", null, false, "ec25fed2-f5c7-4305-b259-e9bb85ba052a", false, "alper" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("2798212b-3e5d-4556-8629-a64eb70da4a8"));
        }
    }
}
