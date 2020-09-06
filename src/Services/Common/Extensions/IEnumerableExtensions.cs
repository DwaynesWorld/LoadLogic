using System.Collections.Generic;
using System.Linq;

namespace LoadLogic.Services.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Count() == 0;
        }
    }
}
