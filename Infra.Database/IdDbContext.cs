using Infra.Database.Entities;
using Infra.Database.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database
{
    public class IdDbContext : DbContext
    {
        public IdDbContext(DbContextOptions<IdDbContext> options) 
            : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserEntityConfiguration());
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        DbSet<UserEntity> UserEntities { get; set; }
    }
}