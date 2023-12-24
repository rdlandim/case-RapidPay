using DAL.RapidPay.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.RapidPay.Security;

namespace DAL.RapidPay.EntitiesConfiguration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            builder.HasIndex(e => e.Email).IsUnique();

            builder.HasData(new User
            {
                Id = 1,
                Name = "Test User",
                Email = "user@test.com",
                Password = "T3stus3R".ToSha256(),
            });
        }
    }
}
