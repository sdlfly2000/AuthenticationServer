namespace Infra.Core.ApplicationBasics
{
    public abstract record AppResponse(string ErrorMessage, bool Success);
}
