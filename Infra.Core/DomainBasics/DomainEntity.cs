using Common.Core.AOP.Cache;

namespace Infra.Core.DomainBasics
{
    public abstract class DomainEntity
    {
        public IReference Id { get; set; }

        protected DomainEntity(IReference id)
        {
            Id = id;
        }
    }
}
