using Application.Services.ReqRes;
using Application.Services.User.Queries;
using Infra.Core.RequestTrace;
using Infra.Core.Test;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Application.Services.Tests.User.Queries
{
    [TestClass]
    public class GetAllClaimTypesRequestHandlerTest
    {
        private GetAllClaimTypesRequestHandler _getAllClaimTypesRequestHandler;
        private IServiceProvider _serviceProvider;

        [TestInitialize]
        public void TestInitialize()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<ILogger>((service) => Log.Logger);
            serviceCollection.AddTransient<IRequestTraceService>((service) => new RequestTraceService { TraceId = "TraceId" });
            _serviceProvider = serviceCollection.BuildServiceProvider();

            _getAllClaimTypesRequestHandler = new GetAllClaimTypesRequestHandler(_serviceProvider);
        }

        [TestMethod, TestCategory(nameof(TestCategoryType.UnitTest))]
        public async Task Given_GetClaimTypesRequest_When_handle_Then_GetClaimTypesResponse_return()
        {
            // Arrange

            // Action
            var response = await _getAllClaimTypesRequestHandler.Handle(new GetClaimTypesRequest(), CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
        }
    }
}
