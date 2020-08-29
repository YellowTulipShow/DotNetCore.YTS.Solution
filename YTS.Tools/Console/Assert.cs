using System;

namespace YTS.Tools
{
    public static class Assert
    {
        public static bool IsEqual<T>(this T source, T verification, Func<T, T, bool> method)
        {
            bool result = method(source, verification);
            if (result == false)
            {
                Console.WriteLine("错误不相等:");
                Console.WriteLine("source:{0}", source);
                Console.WriteLine("verification:{0}", verification);
            }
            return result;
        }

        public static bool IsEqual(this int source, int verification)
        {
            return IsEqual(source, verification, (s, v) => s == v);
        }
        public static bool IsEqual(this double source, double verification)
        {
            return IsEqual(source, verification, (s, v) => s == v);
        }
        public static bool IsEqual(this decimal source, decimal verification)
        {
            return IsEqual(source, verification, (s, v) => s == v);
        }
        public static bool IsEqual(this string source, string verification)
        {
            return IsEqual(source, verification, (s, v) => s == v);
        }
        public static bool IsEqual(this bool source, bool verification)
        {
            return IsEqual(source, verification, (s, v) => s == v);
        }
    }
}
