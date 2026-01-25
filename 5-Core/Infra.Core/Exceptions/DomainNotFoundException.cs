using System.Diagnostics.CodeAnalysis;

namespace Infra.Core.Exceptions;

public class DomainNotFoundException: SystemException
{
    public DomainNotFoundException(string domainOjectName, string by) : base($"Domain Object {domainOjectName} is Not Found by {by}.")
    {        
    }

    public static void ThrowIfNull([NotNull] object? domainOject, string domainOjectName, string by)
    {
        if (domainOject is null)
        {
            throw new DomainNotFoundException(domainOjectName, by);
        }
    }
}
