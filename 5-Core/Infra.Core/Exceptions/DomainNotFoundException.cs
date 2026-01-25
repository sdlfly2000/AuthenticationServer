namespace Infra.Core.Exceptions;

public class DomainNotFoundException: SystemException
{
    public DomainNotFoundException(string domainOjectName, string by) : base($"Domain Object {domainOjectName} is Not Found by {by}.")
    {        
    }
}
