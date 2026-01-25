using Common.Core.Authentication;
using Common.Core.CQRS;
using Common.Core.DependencyInjection;
using Domain.Role.Entities;
using Domain.Role.Repositories;
using Infra.Core.Exceptions;
using Infra.Core.Test;
using Infra.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.Moq;
using Moq;
using Serilog;
using System.Threading.Tasks;

namespace Infra.Database.Tests.Repositories;

[TestClass]
public class RoleRepositoryTest
{
    private RoleRepository _roleRepository;
    private Mock<IdDbContext> _dbContextMock;

    private Guid _roleId = Guid.NewGuid();
    private List<Role> _roles;
    private List<Right> _rights;

    [TestInitialize]
    public void TestInitialize()
    {
        _rights = new List<Right>
        {
            new Right { Id = Guid.NewGuid(), RightName = "Read" },
            new Right { Id = Guid.NewGuid(), RightName = "Write" }
        };

        _roles = new List<Role> 
        {
            new Role 
            { 
                Id = _roleId, 
                RoleName = "Admin", 
                Rights = _rights
            }
        };

        var dbOptions = new DbContextOptions<IdDbContext>();
        _dbContextMock = new Mock<IdDbContext>(dbOptions);

        _dbContextMock
            .Setup(db => db.Set<Role>())
            .Returns(_roles.BuildMockDbSet().Object);

        _roleRepository = new RoleRepository(_dbContextMock.Object);
    }

    [TestMethod(), TestCategory(nameof(TestCategoryType.UnitTest))]
    public async Task Given_RoleId_When_Get_Then_Role_with_Rights_return()
    {
        // Arrange

        // Action
        var result = await _roleRepository.Get(_roleId, CancellationToken.None).ConfigureAwait(false);

        // Asserts
        Assert.IsNotNull(result);
        Assert.AreEqual(_roleId, result.Id);
        Assert.IsNotNull(result.Rights);
        Assert.HasCount(2, result.Rights);
    }

    [TestMethod(), TestCategory(nameof(TestCategoryType.UnitTest))]
    public async Task Given_RoleIdNotExist_When_Get_Then_ExceptionThrown()
    {
        // Arrange
        try
        {
            // Action
            var result = await _roleRepository.Get(Guid.NewGuid(), CancellationToken.None).ConfigureAwait(false);
        }
        catch(Exception ex)
        {
            // Asserts
            Assert.IsInstanceOfType(ex, typeof(DomainNotFoundException));
        }
    }

    [TestMethod(), TestCategory(nameof(TestCategoryType.SystemTest))]
    public async Task Given_RoleId_When_Add_Then_SaveRoleToDb()
    {
        // Arrange
        var serviceCollection = CreateServiceCollectionWithTestDb();
        using var serviceProvider = serviceCollection.BuildServiceProvider();
        var roleRepository = serviceProvider.GetRequiredService<IRoleRepository>();
        var rightRepository = serviceProvider.GetRequiredService<IRightRepository>();
        var roleId = Guid.NewGuid();
        var rightId = Guid.NewGuid();
        var right = new Right
        {
            Id = rightId,
            RightName = "Read"
        };

        var role = new Role
        {
            Id = roleId,
            RoleName = "Admin",
            Rights = new List<Right>
            {
                right
            }
        };

        // Action
        var roleSaved = await roleRepository.Add(role, CancellationToken.None).ConfigureAwait(false);
        var savedNumber = await roleRepository.Save(CancellationToken.None).ConfigureAwait(false);

        // Asserts
        Assert.IsNotNull(roleSaved);
        Assert.IsGreaterThan(0, savedNumber);

        var roleLoaded = await roleRepository.Get(role.Id, CancellationToken.None).ConfigureAwait(false);
        Assert.IsNotNull(roleLoaded);
        Assert.AreEqual(role.Id, roleLoaded.Id);
        Assert.AreEqual(role.RoleName, roleLoaded.RoleName);
        Assert.HasCount(1, roleLoaded.Rights);
        Assert.AreEqual("Read", roleLoaded.Rights[0].RightName);

        // Clean up
        roleRepository.Delete(roleLoaded);
        await roleRepository.Save(CancellationToken.None).ConfigureAwait(false);
        await rightRepository.Delete(right, CancellationToken.None).ConfigureAwait(false);
    }

    #region Private Methods

    private IServiceCollection CreateServiceCollectionWithTestDb()
    {
        var configuration = new ConfigurationManager()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var serviceCollection = new ServiceCollection();

        // Register Configuration
        serviceCollection.AddScoped<IConfiguration>(sp => configuration);
        // Register Services
        serviceCollection
            .RegisterDomain("Infra.Database", "Infra.Shared.Core", "Infra.Core");
        // Test Database
        serviceCollection.AddDbContextPool<IdDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("IdentityDatabaseTest"))
        );

        return serviceCollection;
    }

    #endregion
}
