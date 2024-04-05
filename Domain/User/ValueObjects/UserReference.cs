using Common.Core.AOP.Cache;
using Infra.Core.CacheFieldNames;

namespace Domain.User.ValueObjects
{
    public class UserReference : IReference
    {
        private string _cacheFieldName = CacheFieldNames.User;

        public string Code { get; set; }

        public string CacheFieldName { get => _cacheFieldName; set => _cacheFieldName = value; }

        public string CacheCode => string.Concat(CacheFieldName,Code);

        public UserReference(string code)
        {
            Code = code;
        }

        public static UserReference Create(string code)
        {
            return new UserReference(code);
        }

        public static explicit operator UserReference(string code)
        {
            return new UserReference(code);
        }
    }
}
