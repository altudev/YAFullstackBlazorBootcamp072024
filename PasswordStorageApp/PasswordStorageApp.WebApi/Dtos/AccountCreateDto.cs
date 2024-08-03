using System.ComponentModel.DataAnnotations;
using PasswordStorageApp.WebApi.Models;

namespace PasswordStorageApp.WebApi.Dtos
{
    public class AccountCreateDto
    {
        [Required,MinLength(6),MaxLength(40)]
        public string Username { get; set; }

        [Required, MinLength(6), MaxLength(40)]
        public string Password { get; set; }

        public Account ToAccount()
        {
            return new Account
            {
                Id = Ulid.NewUlid().ToGuid(),
                Username = Username,
                Password = Password,
                CreatedOn = DateTimeOffset.UtcNow
            };
        }
    }
}
