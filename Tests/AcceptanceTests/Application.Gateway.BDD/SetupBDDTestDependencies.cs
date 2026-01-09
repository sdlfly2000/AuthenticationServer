using Common.Core.CQRS;
using Common.Core.DependencyInjection;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll.Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Application.Gateway.BDD
{
    internal class SetupBDDTestDependencies
    {
        [ScenarioDependencies]
        public static IServiceCollection CreateServices()
        {
            var configuration = new ConfigurationManager()
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();

            var serviceCollection = new ServiceCollection();

            // Register Services
            serviceCollection
                .RegisterDomain("Application.Gateway", "Application.Services", "Infra.Database", "Infra.Shared.Core", "Infra.Core")
                .RegisterNotifications("Application.Services");

            // Test Database
            serviceCollection.AddDbContextPool<IdDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("IdentityDatabaseTest"))
            );

            // Add Serilog
            serviceCollection.AddSerilog(
                (configure) =>
                    configure.ReadFrom.Configuration(configuration));

            return serviceCollection;
        }
    }
}
