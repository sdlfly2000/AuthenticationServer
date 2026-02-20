using Common.Core.CQRS.Request;
using Infra.Core.ApplicationBasics;

namespace Application.Services.ReqRes
{
    public record GetAllAppNamesRequest : AppRequest, IRequest;

    public record GetAllAppNamesResponse(string Message, bool Success, IList<string> appNames) : AppResponse(Message, Success), IResponse;
}
