using Common.Core.CQRS.Request;
using Infra.Core.ApplicationBasics;

namespace Application.Services.User.ReqRes;

public record AuthorizeRequest(params string[] Rights) : AppRequest, IRequest
{

}

public record AuthorizeResponse : AppResponse, IResponse
{
    public AuthorizeResponse( string ErrorMessage, bool Success) : base(ErrorMessage, Success)
    {
    }

    public AuthorizeResponse(string ErrorMessage, bool Success, bool IsAuthorized) : base(ErrorMessage, Success)
    {        
        this.IsAuthorized = IsAuthorized;
    }

    public bool IsAuthorized { get; init; }
}
