using Application.Services.User.Queries;
using Application.Services.User.ReqRes;
using Infra.Core.Test;

namespace Application.Services.Tests.User.Queries
{
    [TestClass]
    public class GetAllClaimTypesRequestHandlerTest
    {
        private GetAllClaimTypesRequestHandler _getAllClaimTypesRequestHandler;

        [TestInitialize]
        public void TestInitialize()
        {
            _getAllClaimTypesRequestHandler = new GetAllClaimTypesRequestHandler();
        }

        [TestMethod, TestCategory(nameof(TestCategoryType.UnitTest))]
        public async Task Given_GetClaimTypesRequest_When_handle_Then_GetClaimTypesResponse_return()
        {
            // Arrange

            // Action
            var response = await _getAllClaimTypesRequestHandler.Handle(new GetClaimTypesRequest());

            // Assert
            Assert.IsNotNull(response);
        }
    }
}
