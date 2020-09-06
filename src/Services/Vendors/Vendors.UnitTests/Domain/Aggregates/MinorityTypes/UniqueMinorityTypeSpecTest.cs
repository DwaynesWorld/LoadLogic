using System;
using LoadLogic.Services.Vendors.Domain;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests.Aggregates
{
    public class UniqueMinorityTypeSpecTest
    {
        [Fact]
        public void UniqueMinorityTypeSpec_ShouldCreate()
        {
            //Given 
            var code = "TEST";

            //When
            var spec = new UniqueMinorityTypeSpec(code);

            //Then 
            Assert.NotNull(spec);
        }

        [Fact]
        public void UniqueMinorityTypeSpec_ShouldCheckSatificationOfExpression()
        {
            //Given 
            var code = "TEST";
            var description = "Testing";
            var type = new MinorityType(code, description);



            var code2 = "TEST2";
            var description2 = "Testing 2";
            var type2 = new MinorityType(code2, description2);

            //When
            var spec = new UniqueMinorityTypeSpec(code);

            //Then 
            Assert.NotNull(spec);
            Assert.True(spec.IsSatisfiedBy(type));
            Assert.False(spec.IsSatisfiedBy(type2));
        }
    }
}
