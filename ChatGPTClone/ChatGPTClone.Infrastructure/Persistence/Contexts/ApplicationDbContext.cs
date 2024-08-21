using ChatGPTClone.Application.Common.Interfaces;
using ChatGPTClone.Domain.Entities;
using ChatGPTClone.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatGPTClone.Infrastructure.Persistence.Contexts;

public class ApplicationDbContext:IdentityDbContext<AppUser,Role,Guid,AppUserClaim,AppUserRole,AppUserLogin,RoleClaim,AppUserToken>, IApplicationDbContext
{
    public DbSet<ChatSession> ChatSessions { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}