using Common.Core.AOP.Cache;

namespace Infra.Core.DomainBasics
{
    public abstract class DomainEntity
    {
        public IReference Id { get; private set; }

        protected DomainEntity(IReference id)
        {
            Id = id;
        }
    }
}
