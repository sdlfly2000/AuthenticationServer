using Common.Core.CQRS.Request;

namespace Application.Services.ReqRes
{
    public interface ICacheRequest : IRequest
    {
        string Id { get; }
    }
}
