using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    internal static class EmptyList<T>
    {
        public static readonly T[] Value = new T[0];
    }
}
