using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public static class StringHelper
    {
        public static string Beautify(string message)
        {
            return Regex.Replace(message, "((?<=[a-z])(?=[A-Z]))|((?<=[A-Z])(?=[A-Z][a-z]))", " ");
        }

        private static readonly char[] Splitter = ",;".ToCharArray();
        private static readonly string[] EmptyArray = new string[0];

        public static string[] ToArray(string s)
        {
            if (String.IsNullOrWhiteSpace(s))
                return EmptyArray;

            return s.Split(Splitter, StringSplitOptions.RemoveEmptyEntries);
        }
        public static string Combine(string[] arrays)
        {
            if (arrays == null || arrays.Length == 0)
                return String.Empty;

            return String.Join(",", arrays);
        }
    }
}
