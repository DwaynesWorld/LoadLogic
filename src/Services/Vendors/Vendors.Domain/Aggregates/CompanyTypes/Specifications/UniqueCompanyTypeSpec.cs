using System;
using System.Linq.Expressions;

namespace LoadLogic.Services.Vendors.Domain
{
    public class UniqueCompanyTypeSpec : SpecificationBase<CompanyType>
    {
        private readonly string _code;

        public UniqueCompanyTypeSpec(string code)
        {
            _code = code.ToUpper();
        }

        public override Expression<Func<CompanyType, bool>> SpecExpression
        {
            get { return c => c.Code == _code; }
        }
    }
}
