namespace Infra.Core.Exceptions;

public class UnauthorizedException : SystemException
{
    public UnauthorizedException(string unauthorizedOperation) : base($"Operation: {unauthorizedOperation} is not Authorized")
    {        
    }

    public static void Throw(string unauthorizedOperation)
    {
        throw new UnauthorizedException(unauthorizedOperation);        
    }
}
