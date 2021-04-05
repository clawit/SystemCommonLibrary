using System.Collections;

namespace SystemCommonLibrary.Valid
{
    public static class CollectionValidExtension
    {
        public static bool IsEmpty(this ICollection value)
        {
            return value == null || value.Count == 0;
        }

        public static bool NotEmpty(this ICollection value)
        {
            return value != null && value.Count > 0;
        }

    }
}
