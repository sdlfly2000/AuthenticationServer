using Common.Core.AOP.Cache;

namespace Infra.Core.DomainBasics
{
    public interface DomainEntity<out TReference> where TReference : IReference
    {
        public TReference Id { get; }
    }
}
