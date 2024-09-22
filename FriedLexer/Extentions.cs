using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriedLexer
{
    public static class Extentions
    {
        public static string GetName<TokenEnum>(this TokenEnum @enum) where TokenEnum : System.Enum
        {
            return System.Enum.GetName(typeof(TokenEnum), @enum) ?? string.Empty;
        }
        public static int GetLongestLength(this IEnumerable<string> lst)
        {
            int longest = 0;
            foreach (var key in lst)
            {
                if (key.Length > longest)
                {
                    longest = key.Length;
                }
            }
            return longest;
        }
    }
}
