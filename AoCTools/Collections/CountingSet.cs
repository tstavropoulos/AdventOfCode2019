using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoCTools.Collections
{
    public class CountingSet<T> : IEnumerable<KeyValuePair<T, int>>, IEnumerable
    {
        private readonly Dictionary<T, int> counts;

        /// <summary>
        /// Enumeration of all of unique values submitted to the set
        /// </summary>
        public IEnumerable<T> UniqueValues => counts.Keys;

        /// <summary>
        /// Enumeration of every value submitted to the set, including duplicates
        /// </summary>
        public IEnumerable<T> AllValues
        {
            get
            {
                foreach (var kvp in counts)
                {
                    for (int i = 0; i < kvp.Value; i++)
                    {
                        yield return kvp.Key;
                    }
                }
            }
        }

        public IEnumerable<int> UniqueCounts => counts.Values;

        public int this[T item]
        {
            get => counts.ContainsKey(item) ? counts[item] : 0;
            set
            {
                if (value <= 0 && counts.ContainsKey(item))
                {
                    counts.Remove(item);
                }
                else
                {
                    counts[item] = value;
                }
            }
        }

        /// <summary>
        /// Accesses the count property of the Dictionary - Fast
        /// </summary>
        public int UniqueCount => counts.Count;

        /// <summary>
        /// Calculated as a sum of all the counts - Slow
        /// </summary>
        public int Count => counts.Values.Sum();

        public CountingSet()
        {
            counts = new Dictionary<T, int>();
        }

        public CountingSet(IEnumerable<T> values)
        {
            counts = new Dictionary<T, int>();

            AddRange(values);
        }

        public bool ContainsValue(T item) => counts.ContainsKey(item);
        public bool ContainsCount(int count) => counts.ContainsValue(count);

        public void Clear() => counts.Clear();

        /// <summary>
        /// Adds the item and returns the count of the item in the collection
        /// </summary>
        public int Add(T item)
        {
            if (counts.ContainsKey(item))
            {
                return ++counts[item];
            }

            counts[item] = 1;
            return 1;
        }

        /// <summary>
        /// Adds all the items to the collection
        /// </summary>
        public void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                if (counts.ContainsKey(item))
                {
                    ++counts[item];
                }
                else
                {
                    counts[item] = 1;
                }
            }
        }

        /// <summary>
        /// Removes all the items from the collection
        /// </summary>
        public void RemoveRange(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                if (!counts.ContainsKey(item))
                {
                    continue;
                }

                if (--counts[item] == 0)
                {
                    counts.Remove(item);
                }
            }
        }

        /// <summary>
        /// Removes the item and returns the remaining count of the item in the collection.
        /// Returns -1 if the item was never in the collection
        /// </summary>
        public int Remove(T item)
        {
            if (!counts.ContainsKey(item))
            {
                return -1;
            }

            int count = --counts[item];

            if (count == 0)
            {
                counts.Remove(item);
            }

            return count;
        }

        /// <summary>
        /// Removes all matching items and returns the number removed by this operation.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int RemoveAll(T item)
        {
            if (!counts.ContainsKey(item))
            {
                return 0;
            }

            int count = counts[item];
            counts.Remove(item);
            return count;
        }

        public IEnumerator<KeyValuePair<T, int>> GetEnumerator() => counts.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => counts.GetEnumerator();
    }
}
