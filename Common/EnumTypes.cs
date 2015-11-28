using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class EnumTypes
    {
        public static T ToEnum<T>(string value) where T : struct
        {
            return (T)Enum.Parse(typeof(T), value);
        }
        public static T ToEnum<T>(string value, T source) where T : struct
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        public static T ToEnum<T>(long value) where T : struct
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        public static T ToEnum<T>(long value, T source) where T : struct
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
