using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Database;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddDatabase(this IServiceCollection serviceCollection, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            return serviceCollection;
        }
        
        return serviceCollection.AddDbContextPool<IdDbContext>(
            options => options.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly("Infra.Database"))
        );
    }
}
