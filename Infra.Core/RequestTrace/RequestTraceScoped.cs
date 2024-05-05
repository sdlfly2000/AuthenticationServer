namespace Infra.Core.RequestTrace
{
    public class RequestTraceScoped : IDisposable
    {
        public static string TraceId { get; set; }

        public void Dispose()
        {
            TraceId = String.Empty;
        }
    }
}
