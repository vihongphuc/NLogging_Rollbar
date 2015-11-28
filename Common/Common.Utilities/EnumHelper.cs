using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public static class EnumHelper<T>
        where T : struct
    {
        public static T Parse(string enumValue)
        {
            return (T)Enum.Parse(typeof(T), enumValue);
        }
        public static T Parse(string enumValue, bool ignoreCase)
        {
            return (T)Enum.Parse(typeof(T), enumValue, ignoreCase);
        }
    }
}
