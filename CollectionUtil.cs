﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMaker
{
    public static class CollectionUtil
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var t in collection) action(t);
        }

        public static T FirstOrDefault<T>(this IEnumerable<T> collection, Func<T, bool> pred, T def)
        {
            try
            {
                return collection.First(pred);
            }
            catch (InvalidOperationException)
            {
                return def;
            }
        }
    }

    public class ArrayEqualityComparer<T> : IEqualityComparer<T[]>
    {
        public bool Equals(T[] x, T[] y)
        {
            return x.Length == y.Length && Enumerable.Range(0, x.Length).All(i => x[i].Equals(y[i]));
        }

        public int GetHashCode(T[] obj)
        {
            int result = 17;
            foreach (var o in obj)
            {
                result = result * 23 + o.GetHashCode();
            }
            return result;
        }
    }

    /// <summary>
    /// キーと複数の値のコレクションを表します
    /// </summary>
    public class MultiDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, List<TValue>> mDictionary;
        /// <summary>
        /// 指定したキーに関連付けられている複数の値を取得または設定します
        /// </summary>
        public List<TValue> this[TKey key]
        {
            get
            {
                if (!mDictionary.ContainsKey(key))
                {
                    mDictionary.Add(key, new List<TValue>());
                }
                return mDictionary[key];
            }
            set { mDictionary[key] = value; }
        }
        /// <summary>
        /// キーを格納しているコレクションを取得します
        /// </summary>
        public Dictionary<TKey, List<TValue>>.KeyCollection Keys
        {
            get { return mDictionary.Keys; }
        }
        /// <summary>
        /// 複数の値を格納しているコレクションを取得します
        /// </summary>
        public Dictionary<TKey, List<TValue>>.ValueCollection Values
        {
            get { return mDictionary.Values; }
        }
        /// <summary>
        /// 格納されているキーと値のペアの数を取得します
        /// </summary>
        public int Count
        {
            get { return mDictionary.Count; }
        }

        public MultiDictionary()
        {
            mDictionary = new Dictionary<TKey, List<TValue>>();
        }

        public MultiDictionary(IEqualityComparer<TKey> comparer)
        {
            mDictionary = new Dictionary<TKey, List<TValue>>(comparer);
        }

        /// <summary>
        /// 指定したキーと値をディクショナリに追加します
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            if (!mDictionary.ContainsKey(key))
            {
                mDictionary.Add(key, new List<TValue>());
            }
            mDictionary[key].Add(value);
        }
        /// <summary>
        /// 指定したキーと複数の値をディクショナリに追加します
        /// </summary>
        public void Add(TKey key, params TValue[] values)
        {
            foreach (var n in values)
            {
                Add(key, n);
            }
        }
        /// <summary>
        /// 指定したキーと複数の値をディクショナリに追加します
        /// </summary>
        public void Add(TKey key, IEnumerable<TValue> values)
        {
            foreach (var n in values)
            {
                Add(key, n);
            }
        }
        /// <summary>
        /// 指定したキーを持つ値を削除します
        /// </summary>
        public bool Remove(TKey key, TValue value)
        {
            return mDictionary[key].Remove(value);
        }
        /// <summary>
        /// 指定したキーを持つ複数の値を削除します
        /// </summary>
        public bool Remove(TKey key)
        {
            return mDictionary.Remove(key);
        }
        /// <summary>
        /// すべてのキーと複数の値を削除します
        /// </summary>
        public void Clear()
        {
            mDictionary.Clear();
        }
        /// <summary>
        /// 指定したキーと値が格納されているかどうかを判断します
        /// </summary>
        public bool Contains(TKey key, TValue value)
        {
            return mDictionary[key].Contains(value);
        }
        /// <summary>
        /// 指定したキーが格納されているかどうかを判断します
        /// </summary>
        public bool ContainsKey(TKey key)
        {
            return mDictionary.ContainsKey(key);
        }
        /// <summary>
        /// 反復処理する列挙子を返します
        /// </summary>
        public IEnumerator<KeyValuePair<TKey, List<TValue>>> GetEnumerator()
        {
            foreach (var n in mDictionary)
            {
                yield return n;
            }
        }
    }
}
