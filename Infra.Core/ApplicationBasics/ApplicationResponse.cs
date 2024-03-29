namespace Infra.Core.ApplicationBasics
{
    public class ApplicationResponse
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
    }

    public record ApplicaitonResponseRecord(string message, bool success);
}
