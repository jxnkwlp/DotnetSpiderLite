using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite
{
    static class Extensions
    {
        public static Dictionary<TKey, TValue> Marge<TKey, TValue>(this Dictionary<TKey, TValue> current, Dictionary<TKey, TValue> from)
        {
            foreach (var item in from)
            {
                current[item.Key] = item.Value;
            }

            return current;
        }
    }
}
