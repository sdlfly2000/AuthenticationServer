using Common.Core.CQRS.Request;

namespace Application.Services.ReqRes
{
    public record GetClaimTypesRequest() : IRequest;

    public record GetClaimTypesResponse(string Message, bool Success, IList<ClaimTypeValues> ClaimTypes) : IResponse;

    public record ClaimTypeValues(string TypeShortName, string TypeName);
}
