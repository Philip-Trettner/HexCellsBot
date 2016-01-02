using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexCellsBot
{
    public static class Extensions
    {
        public static int FirstIndexOf<T>(this IReadOnlyList<T> lhs, T obj)
        {
            for (var i = 0; i < lhs.Count; ++i)
                if (Equals(lhs[i], obj))
                    return i;
            return -1;
        }
        public static int LastIndexOf<T>(this IReadOnlyList<T> lhs, T obj)
        {
            for (var i = lhs.Count - 1; i >= 0; --i)
                if (Equals(lhs[i], obj))
                    return i;
            return -1;
        }
    }
}
