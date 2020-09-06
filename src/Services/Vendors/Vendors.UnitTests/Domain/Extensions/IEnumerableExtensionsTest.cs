// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Xunit;

// namespace LoadLogic.Services.Vendors.UnitTests.Extensions
// {
//     public class IEnumerableExtensionsTest
//     {
//         [Fact]
//         public void IsEmpty_ShouldReturnTrue_WhenEnumerablesCountIsZero()
//         {
//             var e = new TestEnumerable<string>();
//             Assert.True(e.IsEmpty());
//         }

//         [Fact]
//         public void IsEmpty_ShouldReturnFalse_WhenEnumerablesCountIsGreaterThanZero()
//         {
//             var e = new TestEnumerable<string>("test");
//             Assert.False(e.IsEmpty());
//         }

//         [Fact]
//         public void IsEmpty_ShouldReturnFalse_WhenEnumerablesIsNull()
//         {
//             TestEnumerable<string> e = null;
//             Assert.Throws<ArgumentNullException>(() => e.IsEmpty());
//         }

//         private class TestEnumerable<T> : IEnumerable<T>
//         {
//             private readonly T[] _arr;

//             public TestEnumerable(params T[] things)
//             {
//                 _arr = things;
//             }

//             public IEnumerator<T> GetEnumerator()
//             {
//                 foreach (var item in _arr)
//                 {
//                     yield return item;
//                 }
//             }

//             IEnumerator IEnumerable.GetEnumerator()
//             {
//                 return this.GetEnumerator();
//             }
//         }
//     }
// }
