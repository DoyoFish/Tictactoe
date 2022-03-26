using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doyo.Common
{
    public static class CollectionExtensions
    {
        public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue defaultValue = default(TValue))
        {
            if (dic == null)
            {
                return defaultValue;
            }

            if (dic.ContainsKey(key))
            {
                return dic[key];
            }
            return defaultValue;
        }

        /// <summary>
        /// 得到一个以拼接符连接的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="connectChar"></param>
        /// <returns></returns>
        public static string Connect<T>(this IEnumerable<T> list, char connectChar)
        {
            return list.Connect(connectChar.ToString());
        }

        /// <summary>
        /// 得到一个以拼接符连接的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="connect"></param>
        /// <returns></returns>
        public static string Connect<T>(this IEnumerable<T> list, string connect)
        {
            if (list == null || list.Count() == 0)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            foreach (var item in list)
            {
                builder.AppendFormat("{0}{1}", item.ToString(), connect);
            }
            builder.Length--;
            return builder.ToString();
        }

        public static T GetRandom<T>(this List<T> list)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            if (list.Count == 0)
            {
                return default(T);
            }
            var result = random.Next(0, list.Count);
            return list[result];
        }
    }
}
