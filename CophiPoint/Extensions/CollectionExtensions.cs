using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> values)
        {
            foreach(var x in values ?? new T[0])
            {
                list.Add(x);
            }
        }
        public static void RemoveRange<T>(this IList<T> list, IEnumerable<T> values)
        {
            foreach(var x in values ??new T[0])
            {
                list.Remove(x);
            }
        }
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> collection) => collection ?? new T[0];
    }
}
