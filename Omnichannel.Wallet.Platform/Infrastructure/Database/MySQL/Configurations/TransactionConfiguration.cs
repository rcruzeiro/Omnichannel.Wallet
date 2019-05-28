using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omnichannel.Wallet.Platform.Domain.Accounts;

namespace Omnichannel.Wallet.Platform.Infrastructure.Database.MySQL.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("transactions").HasIndex(t => t.ID);
            builder.Property(t => t.ID).HasColumnName("id");
            builder.Property(t => t.Location).HasColumnName("location");
            builder.Property<int>("operationType").HasColumnName("operation_type").IsRequired();
            builder.Property<int>("eventType").HasColumnName("event_type").IsRequired();
            builder.Property(t => t.Value).HasColumnName("value").IsRequired();
            builder.Property(t => t.CreatedAt).HasColumnName("created_at").IsRequired();

            // relationships
            builder.HasOne(t => t.Account)
                .WithMany(ac => ac.Transactions)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
