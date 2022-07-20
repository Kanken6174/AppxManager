using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppxManager.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains_ex(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public static bool Contains_nocase(this string source, string toCheck)
        {
            return source?.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
