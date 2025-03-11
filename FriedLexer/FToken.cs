using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriedLexer
{
    public struct FToken<TokenEnum> where TokenEnum : System.Enum
    {
        public static string CurrentOriginContext = "none";
        public static string? CurrentInternalMacroContext = null;
        public TokenEnum Type { get; set; }
        public string Origin { get; set; } = CurrentOriginContext;
        public string? InternalMacroName { get; set; } = CurrentInternalMacroContext;
        public int Position { get; set; }
        public int EndPosition => Position + Text.Length;
        public object? Value { get; set; }
        public string Text { get; set; }

        public string SpecialType = "NONE";

        public bool isBadToken => SpecialType == "BAD";
        public bool isEndOfFile => SpecialType == "EOF";
        public bool isNull => SpecialType == "NULL";

        public FToken(TokenEnum type, string specialType = "NONE")
        {
            Type = type;
            Position = 0;
            Value = null;
            Text = string.Empty;
            SpecialType = specialType;
        }

        public FToken(FToken<TokenEnum> source, TokenEnum type)
        {
            Type = type;
            Position = source.Position;
            Value = source.Value;
            Text = source.Text;
            SpecialType = source.SpecialType;
        }
        public FToken(TokenEnum type, int pos, object? val, string txt, string specialType = "NONE")
        {
            Type = type;
            Position = pos;
            Value = val;
            Text = txt;
            SpecialType = specialType;
        }
        public override string ToString()
        {
            return $"token:{Type.GetName()} on pos:{Position} with text:{Text} with val:{Value}";
        }
    }
}
