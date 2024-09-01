namespace ChatGPTClone.Application.Common.Models.Identity
{
    public class IdentityAuthenticateRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public IdentityAuthenticateRequest(string email, string password)
        {
            Email = email;

            Password = password;
        }
    }
}
