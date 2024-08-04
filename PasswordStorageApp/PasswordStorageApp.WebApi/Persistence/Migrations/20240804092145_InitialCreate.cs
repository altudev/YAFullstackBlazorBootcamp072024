using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PasswordStorageApp.WebApi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "CreatedOn", "ModifiedOn", "Password", "Username" },
                values: new object[,]
                {
                    { new Guid("71f3d31c-d0ef-11ee-a506-0242ac120002"), new DateTimeOffset(new DateTime(2023, 3, 15, 9, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "P@ssw0rd123!", "johndoe" },
                    { new Guid("71f3d5f6-d0ef-11ee-a506-0242ac120002"), new DateTimeOffset(new DateTime(2023, 5, 22, 14, 45, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "SecurePass789#", "janedoe" },
                    { new Guid("71f3d75e-d0ef-11ee-a506-0242ac120002"), new DateTimeOffset(new DateTime(2023, 8, 7, 11, 15, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "B0bsC0mpl3xP@ss", "bobsmith" },
                    { new Guid("71f3d88a-d0ef-11ee-a506-0242ac120002"), new DateTimeOffset(new DateTime(2023, 10, 30, 16, 20, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Al1c3J0hns0n2023!", "alicejohnson" },
                    { new Guid("71f3d9a2-d0ef-11ee-a506-0242ac120002"), new DateTimeOffset(new DateTime(2024, 1, 18, 8, 55, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "M1k3R0ss#2024", "mikeross" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Username",
                table: "Accounts",
                column: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
