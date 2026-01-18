using Infra.Core.DomainBasics;

namespace Domain.Right.ValueObjects;
public class Right : DomainValueObject
{
    public Right(string name, string value)
    {
        RoleName = name;
        RightName = value;
    }

    public string RoleName { get; private set; }
    public string RightName { get; private set; }
}
