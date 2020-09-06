using System;
using System.Linq.Expressions;

namespace LoadLogic.Services.Vendors.Domain
{
    public class UniqueRegionSpec : SpecificationBase<Region>
    {
        private readonly string _code;

        public UniqueRegionSpec(string code)
        {
            _code = code.ToUpper();
        }

        public override Expression<Func<Region, bool>> SpecExpression
        {
            get { return c => c.Code == _code; }
        }
    }
}
