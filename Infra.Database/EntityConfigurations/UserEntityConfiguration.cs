using Domain.User.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Database.EntityConfigurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property("_id").HasColumnName("Id").HasColumnType("nvarchar(36)");
            builder.Property(u => u.UserName).HasColumnType("nvarchar(256)");
            builder.Property(u => u.PasswordHash).HasColumnType("nvarchar(max)");
            builder.Property(u => u.DisplayName).HasColumnType("nvarchar(max)");

            builder.Ignore(u => u.Id);
            builder.Ignore(u => u.Claims);

            builder.HasKey("_id");

            builder.HasMany(u => u.Claims)
                .WithOne()
                .HasForeignKey("_userId");

            builder.ToTable(nameof(User));
        }
    }
}
