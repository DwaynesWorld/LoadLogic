using System;
using System.Linq.Expressions;

namespace LoadLogic.Services.Vendors.Domain
{
    public class UniqueMinorityTypeSpec : SpecificationBase<MinorityType>
    {
        private readonly string _code;

        public UniqueMinorityTypeSpec(string code)
        {
            _code = code.ToUpper();
        }

        public override Expression<Func<MinorityType, bool>> SpecExpression
        {
            get { return c => c.Code == _code; }
        }
    }
}
