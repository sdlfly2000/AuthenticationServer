using Common.Core.DependencyInjection;
using Domain.User.Entities;
using Domain.User.Persistors;
using Domain.User.Repositories;
using Infra.Core.Test;
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

        [TestMethod, TestCategory(nameof(TestCategoryType.SystemTest))]
        public async Task Load_A_User()
        {
            // Arrange
            using var services = _serviceCollection?.BuildServiceProvider();
            var userName = "sdlfly2000";
            var password = "2000419736";
            var userRepository = services?.GetRequiredService<IUserRepository>();

            // Action
            var user = await userRepository!.FindUserByUserNamePwd(userName, password);

            Assert.IsNotNull(user);
            Assert.AreEqual("d3f2252e-6058-4d41-9de5-9d8c1f52abcb", user.Id.Code);
        }
    }
}