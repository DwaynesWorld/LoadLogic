using System.Collections.Generic;

namespace LoadLogic.Services.Extensions
{
    public static class ICollectionExtensions
    {
        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            return collection.Count == 0;
        }
    }
}
