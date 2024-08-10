using PasswordStorageApp.Domain.Enums;
using PasswordStorageApp.Domain.Models;

namespace PasswordStorageApp.Domain.Dtos
{
    public class AccountGetByIdDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public AccountType Type { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }

        public static AccountGetByIdDto MapFromAccount(Account account)
        {
            return new AccountGetByIdDto
            {
                Id = account.Id,
                Username = account.Username,
                Password = account.Password,
                Type = account.Type,
                CreatedOn = account.CreatedOn,
                ModifiedOn = account.ModifiedOn
            };
        }

        public AccountUpdateDto ToUpdateDto()
        {
            return new AccountUpdateDto
            {
                Id = Id,
                Username = Username,
                Password = Password,
                Type = Type
            };
        }
    }
}
