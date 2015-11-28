using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public static class Empty<T>
    {
        public static readonly T[] Array = new T[0];
        public static readonly IList<T> List = Array;
        public static readonly IEnumerable<T> Enumerable = Array;
    }
    public static class Empty<TKey, TValue>
    {
        public static readonly IDictionary<TKey, TValue> Dictionary = new Dictionary<TKey, TValue>();
    }
}
