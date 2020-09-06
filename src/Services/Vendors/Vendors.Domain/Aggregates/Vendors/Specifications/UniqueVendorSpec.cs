using System;
using System.Linq.Expressions;

namespace LoadLogic.Services.Vendors.Domain
{
    public class UniqueVendorSpec : SpecificationBase<Vendor>
    {
        private readonly string _code;

        public UniqueVendorSpec(string code)
        {
            _code = code.ToUpper();
        }

        public override Expression<Func<Vendor, bool>> SpecExpression
        {
            get { return c => c.Code == _code; }
        }
    }
}
