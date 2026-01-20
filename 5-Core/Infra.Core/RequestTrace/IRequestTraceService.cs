namespace Infra.Core.RequestTrace
{
    public interface IRequestTraceService
    {
        public string TraceId {  get; set; }
        public Guid CurrentUserId { get; set; }
        public string CurrentUserRole { get; set; }
    }
}
