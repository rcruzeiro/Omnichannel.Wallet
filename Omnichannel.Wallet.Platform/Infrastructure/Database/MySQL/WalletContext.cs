using Core.Framework.Repository;
using Microsoft.EntityFrameworkCore;
using Omnichannel.Wallet.Platform.Domain.Accounts;
using Omnichannel.Wallet.Platform.Infrastructure.Database.MySQL.Configurations;

namespace Omnichannel.Wallet.Platform.Infrastructure.Database.MySQL
{
    public sealed class WalletContext : BaseContext
    {
        public WalletContext(IDataSource source)
            : base(source)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connectionString);
            optionsBuilder.UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // map inherit account types
            modelBuilder.Entity<VoucherAccount>();

            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new AccountConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
