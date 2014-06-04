using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Crosschat //common namespace to see these methods everywhere without usings
{
    public static class EnumerableExtensions
    {        
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source) 
            {
                action(item);
            }
        }

        public static T GetAndRemove<T>(this IDictionary<object, object> source, object key)
        {
            object value;
            if (source.TryGetValue(key, out value))
            {
                source.Remove(key);
                return (T) value;
            }
            else
            {
                throw new KeyNotFoundException(String.Format("object with key {0} doesn't exist", key));       
            }
        }

        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            var result = new List<IEnumerable<T>>();
            var chunk = new List<T>();
            foreach (var item in source)
            {
                if (chunk.Count >= chunksize)
                {
                    result.Add(chunk.ToArray());
                    chunk.Clear();
                }
                chunk.Add(item);
            }
            result.Add(chunk);
            return result;
        }
        
        public static bool Any(this IEnumerable source) 
        {
            foreach (var item in source) 
            {
                return true; //just one call of MoveNext is needed
            }
            return false;
        }

        public static bool IsNullOrEmpty(this IEnumerable enumerable) 
        {
            return enumerable == null || !Any(enumerable);
        }

        public static void RemoveAll<T>(this Collection<T> collection, Func<T, bool> selector)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                var item = collection[i];
                if (selector(item))
                {
                    collection.Remove(item);
                    i--;
                }
            }
        }

        public static void InvokeIfNotNull(this Action action)
        {
            if (action != null)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Unlike the LINQ:ToDictionary it uses indexer[] rather than .Add() method
        /// </summary>
        public static Dictionary<TKey, TElement> ToDictionarySafe<TSource, TKey, TElement>(this IEnumerable<TSource> source, 
            Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            var dictionary = new Dictionary<TKey, TElement>();
            foreach (TSource item in source)
                dictionary[keySelector(item)] = elementSelector(item);
            return dictionary;
        }
    }
}

