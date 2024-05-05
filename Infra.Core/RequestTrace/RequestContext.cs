namespace Infra.Core.RequestTrace
{
    public class RequestContext : IDisposable
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public void Dispose()
        {
            ServiceProvider = default;
        }
    }
}
