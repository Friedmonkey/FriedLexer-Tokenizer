using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriedLexer
{
    [Serializable]
    public class TokenException<TokenEnum> : Exception where TokenEnum : System.Enum
    {
        public FToken<TokenEnum> Token { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public bool Filled { get; protected set; } = false;
        public TokenException()
        { }

        public TokenException(string message, FToken<TokenEnum> token, int line = 0, int column = 0)
    : base(message)
        {
            this.Token = token;
            Line = line;
            Column = column;
            Filled = true;
        }

        public TokenException(string message)
            : base(message)
        { }

        public TokenException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
