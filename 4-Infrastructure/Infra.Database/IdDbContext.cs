using Domain.User.Entities;
using Domain.User.ValueObjects;
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
            builder.ApplyConfiguration(new ClaimEntityConfiguration());
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        DbSet<User> Users { get; set; }
        DbSet<Claim> Claims { get; set; }
    }
}