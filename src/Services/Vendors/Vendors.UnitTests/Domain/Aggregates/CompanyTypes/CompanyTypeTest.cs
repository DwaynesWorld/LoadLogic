using System;
using LoadLogic.Services.Vendors.Domain;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests.Aggregates
{
    public class CompanyTypeTest
    {
        [Fact]
        public void CompanyType_ShouldCreate()
        {
            //Given 
            var code = "TEST";
            var description = "Testing";

            //When
            var type = new CompanyType(code, description);

            //Then 
            Assert.NotNull(type);
        }

        [Fact]
        public void CompanyType_ShouldThrow_WhenCodeIsNullEmptyOrWhiteSpace_DuringCreate()
        {
            //Given 
            var code = string.Empty;
            var description = "Testing";

            //When - Then 
            Assert.Throws<ArgumentException>(() => new CompanyType(code, description));
        }

        [Fact]
        public void CompanyType_ShouldUpdate()
        {
            //Given 
            var code = "TEST";
            var description = "Testing";

            //When
            var type = new CompanyType(code, description);
            type.Update("TEST2", "Testing 2");

            //Then 
            Assert.Equal("TEST2", type.Code);
            Assert.Equal("Testing 2", type.Description);
        }

        [Fact]
        public void CompanyType_ShouldThrow_WhenCodeIsNullEmptyOrWhiteSpace_DuringUpdate()
        {
            //Given 


            var code = "Test";
            var description = "Testing";
            var type = new CompanyType(code, description);

            //When - Then 
            Assert.Throws<ArgumentException>(() => type.Update(string.Empty, "Testing 2"));
        }

        [Fact]
        public void CompanyType_ShouldAlwaysCapitalizeCodes_DuringCreate()
        {
            //Given 
            var code = "test";
            var description = "Testing";

            //When
            var type = new CompanyType(code, description);

            //Then 
            Assert.Equal("TEST", type.Code);
        }

        [Fact]
        public void CompanyType_ShouldAlwaysCapitalizeCodes_DuringUpdate()
        {
            //Given
            var code = "TEST";
            var description = "Testing";

            //When
            var type = new CompanyType(code, description);
            type.Update("test2", "Testing 2");

            //Then 
            Assert.Equal("TEST2", type.Code);
        }
    }
}
