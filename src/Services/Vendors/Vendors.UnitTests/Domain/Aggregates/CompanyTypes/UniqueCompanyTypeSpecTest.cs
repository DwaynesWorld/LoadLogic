using System;
using LoadLogic.Services.Vendors.Domain;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests.Aggregates
{
    public class UniqueCompanyTypeSpecTest
    {
        [Fact]
        public void UniqueCompanyTypeSpec_ShouldCreate()
        {
            //Given 
            var code = "TEST";

            //When
            var spec = new UniqueCompanyTypeSpec(code);

            //Then 
            Assert.NotNull(spec);
        }

        [Fact]
        public void UniqueCompanyTypeSpec_ShouldCheckSatificationOfExpression()
        {
            //Given 


            var code = "TEST";
            var description = "Testing";
            var type = new CompanyType(code, description);

            var code2 = "TEST2";
            var description2 = "Testing 2";
            var type2 = new CompanyType(code2, description2);

            //When
            var spec = new UniqueCompanyTypeSpec(code);

            //Then 
            Assert.NotNull(spec);
            Assert.True(spec.IsSatisfiedBy(type));
            Assert.False(spec.IsSatisfiedBy(type2));
        }
    }
}
