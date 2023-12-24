using DAL.RapidPay.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.RapidPay.EntitiesConfiguration
{
    internal class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
    {
        public void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            builder.ToTable("CreditCards");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            builder.HasIndex(e => e.Number).IsUnique();

            builder.Property(e => e.Balance).HasColumnType("decimal(19,4)").HasDefaultValue(0);

            builder.HasOne(e => e.User)
                .WithMany(u => u.CreditCards)
                .HasForeignKey(e => e.UserId);
        }
    }
}
