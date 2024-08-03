using PasswordStorageApp.WebApi.Models;

namespace PasswordStorageApp.WebApi.Persistence
{
    public static class FakeDbContext
    {
        public static List<Account> Accounts { get; set; } =
        [
            new Account
            {
                Id = Guid.Parse("71f3d31c-d0ef-11ee-a506-0242ac120002"),
                Username = "johndoe",
                Password = "P@ssw0rd123!",
                CreatedOn = DateTimeOffset.Parse("2023-03-15T09:30:00Z")
            },
            new Account
            {
                Id = Guid.Parse("71f3d5f6-d0ef-11ee-a506-0242ac120002"),
                Username = "janedoe",
                Password = "SecurePass789#",
                CreatedOn = DateTimeOffset.Parse("2023-05-22T14:45:00Z")
            },
            new Account
            {
                Id = Guid.Parse("71f3d75e-d0ef-11ee-a506-0242ac120002"),
                Username = "bobsmith",
                Password = "B0bsC0mpl3xP@ss",
                CreatedOn = DateTimeOffset.Parse("2023-08-07T11:15:00Z")
            },
            new Account
            {
                Id = Guid.Parse("71f3d88a-d0ef-11ee-a506-0242ac120002"),
                Username = "alicejohnson",
                Password = "Al1c3J0hns0n2023!",
                CreatedOn = DateTimeOffset.Parse("2023-10-30T16:20:00Z")
            },
            new Account
            {
                Id = Guid.Parse("71f3d9a2-d0ef-11ee-a506-0242ac120002"),
                Username = "mikeross",
                Password = "M1k3R0ss#2024",
                CreatedOn = DateTimeOffset.Parse("2024-01-18T08:55:00Z")
            },
        ];
    }
}
