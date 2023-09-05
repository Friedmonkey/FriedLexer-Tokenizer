using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriedLexer
{
    public abstract class LogicalToken<TokenEnum> : AnalizerBase<char> where TokenEnum : System.Enum
    {
        public LogicalToken() : base('\0') { }


        public string str = string.Empty;
        public int startPos = 0;

        public abstract bool IfMatch();
        public abstract FToken<TokenEnum>? ParseToken();
    }
}
