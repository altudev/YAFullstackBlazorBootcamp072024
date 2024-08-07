using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordStorageApp.Domain.Dtos;
using PasswordStorageApp.WebApi.Persistence.Contexts;

namespace PasswordStorageApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public AccountsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var accounts = await _dbContext
                .Accounts
                .AsNoTracking()
                .Select(ac => AccountGetAllDto.MapFromAccount(ac))
                .ToListAsync(cancellationToken);

            return Ok(accounts);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var account = await _dbContext
                .Accounts
                 .AsNoTracking()
                .FirstOrDefaultAsync(ac => ac.Id == id,cancellationToken);

            if (account is null)
                return NotFound();

            return Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(AccountCreateDto newAccount, CancellationToken cancellationToken)
        {

            var account = newAccount.ToAccount();

            _dbContext
                .Accounts
                .Add(account);

            await _dbContext.SaveChangesAsync(cancellationToken);

            //return Ok(account.Id);

            return Ok(new { data = account.Id, message = "The account was added successfully!" });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, AccountUpdateDto updateDto, CancellationToken cancellationToken)
        {
            if (id != updateDto.Id)
                return BadRequest("The id in the URL does not match the id in the body");

            var account = _dbContext
                .Accounts
                .FirstOrDefault(ac => ac.Id == id);

            var updatedAccount = updateDto.ToAccount(account);

            //_dbContext
            //    .Accounts
            //    .Update(updatedAccount);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok(new { data = updatedAccount, message = "The account was updated successfully!" });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> RemoveAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
             return BadRequest("id is not valid. Please do not send empty guids for god sake!");

            var account = _dbContext
                    .Accounts
                .FirstOrDefault(ac => ac.Id == id);

            if (account is null)
                return NotFound();

            _dbContext
                .Accounts
                .Remove(account);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
