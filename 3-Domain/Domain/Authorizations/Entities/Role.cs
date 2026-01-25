namespace Domain.Authorizations.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string RoleName { get; set; }
    public List<Right> Rights { get; set; } = new List<Right>();

    public bool HasRight(string rightName)
    {
        return !string.IsNullOrEmpty(rightName) && 
            Rights.Any(r => r.RightName.Equals(rightName, StringComparison.InvariantCultureIgnoreCase));
    }

    public void AddRight(Right right)
    {
        ArgumentNullException.ThrowIfNull(right);

        if (!Rights.Any(r => r.Id == right.Id))
        {
            Rights.Add(right);
        }
    }
}
