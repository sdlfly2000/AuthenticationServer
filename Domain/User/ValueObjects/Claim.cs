using Infra.Core.DomainBasics;

namespace Domain.User.ValueObjects
{
    public class Claim : DomainValueObject
    {
        public Claim(string name, string value, string valueType)
        {
            Name = name;
            Value = value;
            ValueType = valueType;
        }

        private string _id { get; }
        public string Name { get; private set; }
        public string Value { get; private set; }
        public string ValueType { get; private set; }
        public string? UserId { get; private set; }
    }
}
