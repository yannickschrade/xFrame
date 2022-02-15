using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.WPF.Extensions
{
    public static class Dictionaries
    {
        public static TValue GetOrSetIfMissing<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue ifNotFound = default)
        {
            if(!dic.ContainsKey(key))
                dic.Add(key, ifNotFound);
            return dic[key];
        }
    }
}
