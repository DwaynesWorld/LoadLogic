using System;
using System.Linq.Expressions;

namespace LoadLogic.Services.Vendors.Domain
{
    public class UniqueProfileSpec : SpecificationBase<Profile>
    {
        public UniqueProfileSpec()
        {
        }

        public override Expression<Func<Profile, bool>> SpecExpression
            => p => false;
    }
}
