namespace PasswordStorageApp.WebApi.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}
