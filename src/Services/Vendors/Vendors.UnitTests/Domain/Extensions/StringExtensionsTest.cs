// using System;
// using System.Collections.Generic;
// using Xunit;

// namespace LoadLogic.Services.Vendors.UnitTests.Extensions
// {
//     public class StringExtensionsTest
//     {
//         [Fact]
//         public void IsValidHexColor_ShouldReturnTrue_WhenHexColorIsValidShortForm() => Assert.True("#EEE".IsValidHexColor());

//         [Fact]
//         public void IsValidHexColor_ShouldReturnTrue_WhenHexColorIsValidLongForm() => Assert.True("#EEEEEE".IsValidHexColor());

//         [Fact]
//         public void IsValidHexColor_ShouldReturnTrue_WhenHexColorIsValidUppercase() => Assert.True("#EEEEEE".IsValidHexColor());

//         [Fact]
//         public void IsValidHexColor_ShouldReturnTrue_WhenHexColorIsValidLowercase() => Assert.True("#eeeeee".IsValidHexColor());

//         [Fact]
//         public void IsValidHexColor_ShouldReturnFalse_WhenHexColorIsInvalidShortForm() => Assert.False("#GGG".IsValidHexColor());

//         [Fact]
//         public void IsValidHexColor_ShouldReturnFalse_WhenHexColorIsInvalidLongForm() => Assert.False("#GGGGGG".IsValidHexColor());

//         [Fact]
//         public void IsValidHexColor_ShouldReturnFalse_WhenHexColorIsInvalidUppercase() => Assert.False("#GGGGGG".IsValidHexColor());

//         [Fact]
//         public void IsValidHexColor_ShouldReturnFalse_WhenHexColorIsInvalidLowercase() => Assert.False("#gggggg".IsValidHexColor());
//     }
// }
