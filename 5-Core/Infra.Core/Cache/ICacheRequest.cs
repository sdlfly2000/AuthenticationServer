using Common.Core.CQRS.Request;

namespace Infra.Core.Cache
{
    public interface ICacheRequest : IRequest
    {
        string Id { get; }
    }
}
