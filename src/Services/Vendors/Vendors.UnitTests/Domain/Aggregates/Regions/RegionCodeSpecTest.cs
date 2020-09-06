using System;
using LoadLogic.Services.Vendors.Domain;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests.Aggregates
{
    public class UniqueRegionSpecTest
    {
        [Fact]
        public void UniqueRegionSpec_ShouldCreate()
        {
            //Given 
            var code = "TEST";

            //When
            var spec = new UniqueRegionSpec(code);

            //Then 
            Assert.NotNull(spec);
        }

        [Fact]
        public void UniqueRegionSpec_ShouldCheckSatificationOfExpression()
        {
            //Given 
            var code = "TEST";
            var description = "Testing";
            var type = new Region(code, description);

            var code2 = "TEST2";
            var description2 = "Testing 2";
            var type2 = new Region(code2, description2);

            //When
            var spec = new UniqueRegionSpec(code);

            //Then 
            Assert.NotNull(spec);
            Assert.True(spec.IsSatisfiedBy(type));
            Assert.False(spec.IsSatisfiedBy(type2));
        }
    }
}
