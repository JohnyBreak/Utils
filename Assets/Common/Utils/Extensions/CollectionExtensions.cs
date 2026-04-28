using System;
using System.Collections.Generic;
using System.Linq;

namespace Source.Scripts
{
    public static class CollectionExtensions
    {
        public static bool IsIdentical<T>(this T[] lhs, T[] rhs)
        {
            if (lhs == null || rhs == null)
            {
                return false;
            }

            if (lhs.Length != rhs.Length)
            {
                return false;
            }

            var comparer = EqualityComparer<T>.Default;

            for (int i = 0; i < lhs.Length; i++)
            {
                if (!comparer.Equals(lhs[i], rhs[i]))
                {
                    return false;
                }
            }

            return true;
        }
        
        public static bool IsIdentical<TKey, TValue>
            (this IReadOnlyDictionary<TKey, TValue> lhs, IReadOnlyDictionary<TKey, TValue> rhs)
        {
            if (lhs == null || rhs == null)
            {
                return false;
            }

            if (lhs.Count != rhs.Count)
            {
                return false;
            }

            var comparer = EqualityComparer<TValue>.Default;

            foreach (var kvp in lhs)
            {
                if (!rhs.TryGetValue(kvp.Key, out var value) || !comparer.Equals(kvp.Value, value))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsIdentical<T>(this IReadOnlyList<T> lhs, IReadOnlyList<T> rhs)
        {
            if (lhs == null || rhs == null)
            {
                return false;
            }

            if (lhs.Count != rhs.Count)
            {
                return false;
            }

            var comparer = EqualityComparer<T>.Default;

            for (int i = 0; i < lhs.Count; i++)
            {
                if (!comparer.Equals(lhs[i], rhs[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsIdentical<T>(this ISet<T> lhs, ISet<T> rhs)
        {
            if (lhs == null || rhs == null)
            {
                return false;
            }

            if (lhs.Count != rhs.Count)
            {
                return false;
            }

            return lhs.Overlaps(rhs);
        }

        public static bool IsIdentical<TKey, TValue>(this IDictionary<TKey, TValue> lhs, IDictionary<TKey, TValue> rhs)
            where TKey : IEquatable<TKey>
            where TValue : IEquatable<TValue>
        {
            if (lhs == null || rhs == null)
            {
                return false;
            }

            if (lhs.Count != rhs.Count)
            {
                return false;
            }

            using var enumerator = lhs.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                
                if (!rhs.TryGetValue(current.Key, out var value) || !value.Equals(current.Value))
                {
                    return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// Topological Sorting (Kahn's algorithm)
        /// </summary>
        /// <remarks>https://en.wikipedia.org/wiki/Topological_sorting</remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodes">All nodes of directed acyclic graph.</param>
        /// <param name="edges">All edges of directed acyclic graph.</param>
        /// <param name="comparer">comparer</param>
        /// <returns>Sorted node in topological order.</returns>
        public static List<T> TopologicalSort<T>(this ISet<T> nodes, HashSet<(T, T)> edges, Func<T, T, bool> comparer)
        {
            // Empty list that will contain the sorted elements
            var sortedList = new List<T>();

            // Set of all nodes with no incoming edges
            var set = new HashSet<T>(nodes.Where(o => edges.All(e => !comparer(e.Item2, o))));

            // while S is non-empty do
            while(set.Any())
            {
                //  remove a node n from S
                var node = set.First();
                set.Remove(node);

                // add n to tail of L
                sortedList.Add(node);

                // for each node m with an edge e from n to m do
                foreach(var e in edges.Where(e => comparer(e.Item1, node)).ToList())
                {
                    var m = e.Item2;

                    // remove edge e from the graph
                    edges.Remove(e);

                    // if m has no other incoming edges then
                    if(edges.All(me => !comparer(me.Item2, m)))
                    {
                        // insert m into S
                        set.Add(m);
                    }
                }
            }

            // if graph has edges then
            if(edges.Any())
            {
                throw new Exception($"Cycle references where found!\n{string.Join(",", edges)}");
            }

            // return sortedList (a topologically sorted order)
            return sortedList;
        }
    }
}