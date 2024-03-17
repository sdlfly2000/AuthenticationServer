using Domain.User.Entities;
using Domain.User.ValueObjects;
using Infra.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Database.EntityConfigurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(u => u.Id).HasColumnName("Id").HasColumnType("nvarchar(256)")
                .HasConversion((reference) => reference.Code, (id) => UserReference.Create(id));
            builder.Property(u => u.UserName).HasColumnType("nvarchar(256)");
            builder.Property(u => u.PasswordHash).HasColumnType("nvarchar(max)");
            builder.Property(u => u.DisplayName).HasColumnType("nvarchar(max)");
            builder.Property(u => u.Status).HasColumnName("Status").HasColumnType("int")
                .HasConversion<int>(status => (int)status, status => (EnumStatus)status);

            builder.ToTable(nameof(User));
            builder.HasKey("Id");
        }
    }
}
