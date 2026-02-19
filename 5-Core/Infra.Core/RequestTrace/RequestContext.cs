using Common.Core.DependencyInjection;

namespace Infra.Core.RequestTrace
{
    [ServiceLocate(typeof(IRequestContext), ServiceType.Scoped)]
    public class RequestContext : IRequestContext
    {
        public string TraceId { get; set; }
        public Guid CurrentUserId { get; set; }
        public List<string> CurrentUserRoles { get; set; }
    }
}
