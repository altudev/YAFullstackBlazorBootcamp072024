using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PasswordStorageApp.Domain.Dtos;
using PasswordStorageApp.WebApi.Persistence.Contexts;

namespace PasswordStorageApp.WebApi.Hubs
{
    public class AccountsHub:Hub
    {
        private readonly ApplicationDbContext _dbContext;

        public AccountsHub(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<AccountGetAllDto>> GetAllAsync()
        {
            var accounts = await _dbContext
                .Accounts
                .AsNoTracking()
                .Select(ac => AccountGetAllDto.MapFromAccount(ac))
                .ToListAsync();

            return accounts;
        }

        public async Task CreateAsync(AccountCreateDto newAccount)
        {

            var account = newAccount.ToAccount();

            _dbContext
                .Accounts
                .Add(account);

            await _dbContext.SaveChangesAsync();

            await Clients
                .AllExcept(Context.ConnectionId)
                .SendAsync("AccountCreated", AccountGetAllDto.MapFromAccount(account));
        }

        public async Task RemoveAsync(Guid id)
        {
            if (id == Guid.Empty)
                return;

            var account = _dbContext
                .Accounts
                .FirstOrDefault(ac => ac.Id == id);

            if (account is null)
                return;

            _dbContext
                .Accounts
                .Remove(account);

            await _dbContext.SaveChangesAsync();

           await Clients
                .AllExcept(Context.ConnectionId)
                .SendAsync("AccountRemoved", id);
        }
    }
}
