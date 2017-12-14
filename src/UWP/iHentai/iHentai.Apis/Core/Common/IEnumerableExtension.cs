using System;
using System.Collections.Generic;
using System.Linq;
using iHentai.Apis.Core.Models.Interfaces;

namespace iHentai.Apis.Core.Common
{
    internal static class IEnumerableExtension
    {
        public static IList<T> WithoutShit<T>(this IEnumerable<T> source) where T : IGalleryModel
        {
            return source.Where(item => !item.Title.Contains("沒有漢化")).ToList();
        }

        public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> source, Func<T, V> keySelector)
        {
            return source.Distinct(new EqualityComparer<T, V>(keySelector));
        }
    }

    internal class EqualityComparer<T, V> : IEqualityComparer<T>
    {
        private readonly Func<T, V> keySelector;

        public EqualityComparer(Func<T, V> keySelector)
        {
            this.keySelector = keySelector;
        }

        public bool Equals(T x, T y)
        {
            return EqualityComparer<V>.Default.Equals(keySelector(x), keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return EqualityComparer<V>.Default.GetHashCode(keySelector(obj));
        }
    }
}