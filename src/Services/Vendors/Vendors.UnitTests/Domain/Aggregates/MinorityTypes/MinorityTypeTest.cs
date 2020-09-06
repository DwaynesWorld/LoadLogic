using System;
using LoadLogic.Services.Vendors.Domain;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests.Aggregates
{
    public class MinorityTypeTest
    {
        [Fact]
        public void MinorityType_ShouldCreate()
        {
            //Given 
            var code = "TEST";
            var description = "Testing";

            //When
            var type = new MinorityType(code, description);

            //Then 
            Assert.NotNull(type);
        }

        [Fact]
        public void MinorityType_ShouldThrow_WhenCodeIsNullEmptyOrWhiteSpace_DuringCreate()
        {
            //Given 
            var code = string.Empty;
            var description = "Testing";

            //When - Then 
            Assert.Throws<ArgumentException>(() => new MinorityType(code, description));
        }

        [Fact]
        public void MinorityType_ShouldUpdate()
        {
            //Given 
            var code = "TEST";
            var description = "Testing";

            //When
            var type = new MinorityType(code, description);
            type.Update("TEST2", "Testing 2");

            //Then 
            Assert.Equal("TEST2", type.Code);
            Assert.Equal("Testing 2", type.Description);
        }

        [Fact]
        public void MinorityType_ShouldThrow_WhenCodeIsNullEmptyOrWhiteSpace_DuringUpdate()
        {
            //Given 
            var code = "TEST";
            var description = "Testing";
            var type = new MinorityType(code, description);

            //When - Then 
            Assert.Throws<ArgumentException>(() => type.Update(string.Empty, "Testing 2"));
        }

        [Fact]
        public void MinorityType_ShouldAlwaysCapitalizeCodes_DuringCreate()
        {
            //Given 
            var code = "test";
            var description = "Testing";

            //When
            var type = new MinorityType(code, description);

            //Then 
            Assert.Equal("TEST", type.Code);
        }

        [Fact]
        public void MinorityType_ShouldAlwaysCapitalizeCodes_DuringUpdate()
        {
            //Given 
            var code = "Test";
            var description = "Testing";

            //When
            var type = new MinorityType(code, description);
            type.Update("test2", "Testing 2");

            //Then 
            Assert.Equal("TEST2", type.Code);
        }
    }
}
