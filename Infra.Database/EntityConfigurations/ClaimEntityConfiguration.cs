using Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Database.EntityConfigurations
{
    public class ClaimEntityConfiguration : IEntityTypeConfiguration<Claim>
    {
        public void Configure(EntityTypeBuilder<Claim> builder)
        {
            builder.Property("_id").HasColumnName("ClaimId").HasColumnType("nvarchar(36)");
            builder.Property("_userId").HasColumnName("UserId").HasColumnType("nvarchar(36)");

            builder.Property(c => c.Name).HasColumnName("Name").HasColumnType("nvarchar(max)");
            builder.Property(c => c.ValueType).HasColumnName("ValueType").HasColumnType("nvarchar(max)");
            builder.Property(c => c.Value).HasColumnName("Value").HasColumnType("nvarchar(max)");

            builder.HasKey("_id");
            builder.HasIndex("_userId");
            builder.ToTable(nameof(Claim));
        }
    }
}
