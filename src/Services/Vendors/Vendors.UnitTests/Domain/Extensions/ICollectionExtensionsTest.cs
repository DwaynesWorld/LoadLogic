// using System;
// using System.Collections.Generic;
// using Xunit;

// namespace LoadLogic.Services.Vendors.UnitTests.Extensions
// {
//     public class ICollectionExtensionsTest
//     {
//         [Fact]
//         public void IsEmpty_ShouldReturnTrue_WhenCollectionsCountIsZero()
//         {
//             var coll = new HashSet<string>();
//             Assert.True(coll.IsEmpty());
//         }

//         [Fact]
//         public void IsEmpty_ShouldReturnFalse_WhenCollectionsCountIsGreaterThanZero()
//         {
//             var coll = new HashSet<string> { "test" };
//             Assert.False(coll.IsEmpty());
//         }

//         [Fact]
//         public void IsEmpty_ShouldReturnFalse_WhenCollectionsIsNull()
//         {
//             HashSet<string> coll = null;
//             Assert.Throws<NullReferenceException>(() => coll.IsEmpty());
//         }
//     }
// }
