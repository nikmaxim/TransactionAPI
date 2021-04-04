using System;
using System.Collections.Generic;
using System.Linq;

namespace TransactionAPI.Services
{
    public static class DistinctByExtension
    {
        /// <summary>
        /// Processes the collection and returns an enumeration with unique values for a specific key
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source">The collection that is processed</param>
        /// <param name="keySelector">The key by which duplicates are removed</param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            source = source.Reverse();
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
