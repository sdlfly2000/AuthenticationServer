using Common.Core.DependencyInjection;
using Domain.User.Entities;
using Domain.User.Repositories;
using Infra.Core.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Database.Tests.Repositories
{
    [TestClass]
    public class UserRepositoryTest
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

        [TestMethod, TestCategory(nameof(TestCategoryType.SystemTest))]
        public async Task Create_A_User()
        {
            // Arrange
            using var services = _serviceCollection?.BuildServiceProvider();
            var userName = "Jay Shi";
            var user = User.Create(userName);
            var userRepository = services.GetRequiredService<IUserRepository>();

            // Action
            var result  = await userRepository.Add(user);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}