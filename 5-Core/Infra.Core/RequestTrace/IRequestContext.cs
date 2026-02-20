namespace Infra.Core.RequestTrace
{
    public interface IRequestContext
    {
        public string TraceId {  get; set; }
        public Guid CurrentUserId { get; set; }
        public List<string> CurrentUserRoles { get; set; }
    }
}
