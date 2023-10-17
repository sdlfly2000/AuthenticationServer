using Infra.Database.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database
{
    public class IdDbContext : IdentityDbContext<UserEntity, RoleEntity, Guid>
    {
        public IdDbContext(DbContextOptions<IdDbContext> options) 
            : base(options)
        {                
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}