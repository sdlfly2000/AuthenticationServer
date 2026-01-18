namespace Domain.Role.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string RoleName { get; set; }
    public List<Right> Rights { get; set; }
}
