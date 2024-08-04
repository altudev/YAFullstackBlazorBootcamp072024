using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasswordStorageApp.Domain.Enums;
using PasswordStorageApp.Domain.Models;

namespace PasswordStorageApp.WebApi.Persistence.Configurations
{
    public class AccountConfiguration: IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            // ID
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            // Username
            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(50);
            // Username Index
            builder.HasIndex(x => x.Username);

            // Password
            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(50);

            // Type
            builder.Property(x => x.Type)
                .HasConversion<int>()
                .HasDefaultValue(AccountType.Web)
                .IsRequired();

            // CreatedOn
            builder.Property(x => x.CreatedOn)
                .IsRequired();

            // ModifiedOn
            builder.Property(x => x.ModifiedOn)
                .IsRequired(false);

            // Table Name
            builder.ToTable("Accounts");
        }
    }
}
