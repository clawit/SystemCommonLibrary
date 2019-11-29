using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemCommonLibrary.Collection
{
    public static class CollectionExtension
    {
        public static void Foreach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    action(item);
                }
            }
        }

        public static bool NotNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection != null && collection.Any();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) => !collection.NotNullOrEmpty();

    }
}
