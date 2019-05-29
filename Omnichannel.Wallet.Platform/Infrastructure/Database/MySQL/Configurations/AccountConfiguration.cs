using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omnichannel.Wallet.Platform.Domain.Accounts;

namespace Omnichannel.Wallet.Platform.Infrastructure.Database.MySQL.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("accounts").HasIndex(ac => ac.ID);
            builder.Property(ac => ac.ID).HasColumnName("id");
            builder.Property(ac => ac.Company).HasColumnName("company").IsRequired();
            builder.Property(ac => ac.AccountId).HasColumnName("account_id").IsRequired();
            builder.Property<int>("accountType").HasColumnName("account_type").IsRequired();
            builder.Property(ac => ac.CPF).HasColumnName("cpf").IsRequired();
            builder.Property(ac => ac.InitialValue).HasColumnName("initial_value").IsRequired();
            builder.Property(ac => ac.Balance).HasColumnName("balance").IsRequired();
            builder.Property(ac => ac.ExpiresOn).HasColumnName("expires_on");
            builder.Property(ac => ac.CreatedAt).HasColumnName("created_at").IsRequired();
            builder.Property(ac => ac.UpdatedAt).HasColumnName("updated_at");

            //ignore property (for now)
            builder.Ignore(ac => ac.ExtensionAttributes);

            // discriminator
            builder.HasDiscriminator<int>("accountType")
                .HasValue<VoucherAccount>(1);

            // navigation (has to be navigation field because transactions list is read-only)
            var navigation =
                builder.Metadata.FindNavigation(nameof(Account.Transactions));

            // ef access the transactions collection property through its backing field
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            // relationships
            builder.HasMany(ac => ac.Transactions)
                .WithOne(t => t.Account);
        }
    }
}
