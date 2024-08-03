using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PasswordStorageApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var passwords = _passwords;

            return Ok(passwords);
        }

        [HttpGet("{passwordId}")]
        public IActionResult GetById(string passwordId)
        {
            var password = _passwords
                .FirstOrDefault(p => p == passwordId);

            if (string.IsNullOrEmpty(password))
                return NotFound();

            return Ok(password);
        }

        [HttpDelete("{passwordId}")]
        public IActionResult Remove(string passwordId)
        {
            var password = _passwords
                .FirstOrDefault(p => p == passwordId);

            if (string.IsNullOrEmpty(password))
                return NotFound();

            _passwords.Remove(password);
            // dbContext.Passwords.Remove(password);

            return NoContent();
        }

        private static readonly List<string> _passwords = new()
        {
            "123456",
            "password",
            "123456789",
            "12345678",
            "12345",
            "1234567",
            "1234567890",
            "qwerty",
            "abc123",
            "111111",
            "123123",
            "admin",
            "letmein",
            "welcome",
            "monkey",
            "password1",
            "1234",
            "password",
            "superman",
            "iloveyou",
            "123456a",
            "trustno1",
            "1234567",
            "sunshine",
            "master",
            "123123",
            "welcome",
            "shadow",
            "ashley",
            "football",
            "jesus",
            "michael",
            "ninja",
            "mustang",
            "password1",
            "p@ssw0rd",
            "hello",
            "charlie",
            "aa123456",
            "donald",
            "password",
            "qazwsx"
        };
    }
}
