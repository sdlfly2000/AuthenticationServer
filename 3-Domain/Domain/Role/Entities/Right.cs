namespace Domain.Role.Entities;
public class Right
{
    public Right(string name, string value)
    {
        RightName = value;
    }

    public Guid Id { get; set; }
    public string RightName { get; set; }

    private Guid _roleId;
}
