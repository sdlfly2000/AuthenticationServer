using Common.Core.Domain;
using Domain.User.ValueObjects;

namespace Domain.User.Entities;

public class Claim : DomainEntity<ClaimReference>
{
    public Claim(string name, string value, bool isFixed = false)
    {
        Name = name;
        Value = value;
        IsFixed = isFixed;

        _id = Guid.NewGuid().ToString();
        _userId = string.Empty;
    }
    
    public ClaimReference Id => ClaimReference.Create(_id);
    public string Name { get; private set; }
    public string Value { get; private set; }
    public bool IsFixed { get; private set; }

    #region Database Usage

    private string _id { get; set; }
    private string _userId { get; set; }

    #endregion

    public void AssignUser(string userId)
    {
        _userId = userId;
    }

    public void SetValue(string value)
    {
        Value = value;
    }
}