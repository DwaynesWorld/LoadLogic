using System;
using LoadLogic.Services.Vendors.Domain;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests.Aggregates
{
    public class UniqueProductTypeSpecTest
    {
        [Fact]
        public void UniqueProductTypeSpec_ShouldCreate()
        {
            //Given 
            var code = "TEST";

            //When
            var spec = new UniqueProductTypeSpec(code);

            //Then 
            Assert.NotNull(spec);
        }

        [Fact]
        public void UniqueProductTypeSpec_ShouldCheckSatificationOfExpression()
        {
            //Given 
            var code = "TEST";
            var description = "Testing";
            var type = new ProductType(code, description);



            var code2 = "TEST2";
            var description2 = "Testing 2";
            var type2 = new ProductType(code2, description2);

            //When
            var spec = new UniqueProductTypeSpec(code);

            //Then 
            Assert.NotNull(spec);
            Assert.True(spec.IsSatisfiedBy(type));
            Assert.False(spec.IsSatisfiedBy(type2));
        }
    }
}
