using Domain.Role.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Database.EntityConfigurations
{
    public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(c => c.Id).HasColumnName("RoleId").HasColumnType("nvarchar(36)");
            builder.Property(c => c.RoleName).HasColumnName("RoleName").HasColumnType("nvarchar(256)");

            builder.HasKey(c => c.Id);
            builder.HasMany(role => role.Rights)
                   .WithMany();

            builder.ToTable(nameof(Role));
        }
    }
}
