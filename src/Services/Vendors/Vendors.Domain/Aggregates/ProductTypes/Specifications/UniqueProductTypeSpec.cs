using System;
using System.Linq.Expressions;

namespace LoadLogic.Services.Vendors.Domain
{
    public class UniqueProductTypeSpec : SpecificationBase<ProductType>
    {
        private readonly string _code;

        public UniqueProductTypeSpec(string code)
        {
            _code = code.ToUpper();
        }

        public override Expression<Func<ProductType, bool>> SpecExpression
        {
            get { return c => c.Code == _code; }
        }
    }
}
