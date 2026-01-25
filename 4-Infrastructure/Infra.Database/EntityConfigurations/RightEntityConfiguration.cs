using Domain.Authorizations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Database.EntityConfigurations
{
    public class RightEntityConfiguration : IEntityTypeConfiguration<Right>
    {
        public void Configure(EntityTypeBuilder<Right> builder)
        {
            builder.Property(c => c.Id).HasColumnName("RightId").HasColumnType("nvarchar(36)");
            builder.Property(c => c.RightName).HasColumnType("nvarchar(256)");

            builder.HasKey(c => c.Id);
            builder.ToTable(nameof(Right));
        }
    }
}
