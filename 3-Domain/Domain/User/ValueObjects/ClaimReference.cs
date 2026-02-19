using Common.Core.AOP.Cache;
using Infra.Core.CacheFieldNames;

namespace Domain.User.ValueObjects
{
    public class ClaimReference : IReference
    {
        private string _cacheFieldName = CacheFieldNames.Claim;

        public string Code { get; set; }

        public string CacheFieldName { get => _cacheFieldName; set => _cacheFieldName = value; }

        public string CacheCode => string.Concat(CacheFieldName,Code);

        public ClaimReference(string code)
        {
            Code = code;
        }

        public static ClaimReference Create(string code)
        {
            return new ClaimReference(code);
        }

        public static explicit operator ClaimReference(string code)
        {
            return new ClaimReference(code);
        }

        public override bool Equals(object? obj)
        {
            var userReference = obj as ClaimReference;
            return Code.Equals(userReference?.Code);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
