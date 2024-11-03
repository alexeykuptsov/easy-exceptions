namespace EasyExceptions.Yaml.Core
{
    /// <summary>
    /// Supports implementations of <see cref="object.GetHashCode"/> by providing methods to combine two hash codes.
    /// </summary>
    internal static class HashCode
    {
        /// <summary>
        /// Combines two hash codes.
        /// </summary>
        /// <param name="h1">The first hash code.</param>
        /// <param name="h2">The second hash code.</param>
        /// <returns></returns>
        public static int CombineHashCodes(int h1, int h2)
        {
            return ((h1 << 5) + h1) ^ h2;
        }

        public static int CombineHashCodes(int h1, object? o2)
        {
            return CombineHashCodes(h1, GetHashCode(o2));
        }

        private static int GetHashCode(object? obj)
        {
            return obj != null ? obj.GetHashCode() : 0;
        }
    }
}
