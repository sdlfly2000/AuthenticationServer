using Common.Core.CQRS.Request;

namespace Application.Services.User.ReqRes
{
    public interface ICacheRequest : IRequest
    {
        string Id { get; }
    }
}
