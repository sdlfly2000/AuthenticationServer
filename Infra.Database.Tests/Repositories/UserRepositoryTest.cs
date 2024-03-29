using Common.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Database.Tests.Repositories
{
    [TestClass]
    public class UserPersistorTest
    {
        private IServiceCollection? _serviceCollection;

        [TestInitialize]
        public void TestInitialize()
        {
            var connectionString = "server=192.168.71.82;database=Identity;uid=sdlfly2000;password=sdl@1215;TrustServerCertificate=true";
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddDbContextPool<IdDbContext>(
                options => options.UseSqlServer(connectionString)
            );
            _serviceCollection.RegisterDomain("Infra.Database");
        }
    }
}