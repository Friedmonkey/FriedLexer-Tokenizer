using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriedLexer
{
    public static class LogicalTokens
    {
        public class StringToken<EToken> : LogicalToken<EToken> where EToken : System.Enum
        {
            public StringToken(EToken strToken) { this.strToken = strToken; }
            EToken strToken = default;
            string rawSTR = string.Empty;
            public override bool IfMatch()
            {
                rawSTR = string.Empty;
                str = string.Empty;
                startPos = Position;
                return (Current == '"');
            }

            public override FToken<EToken>? ParseToken()
            {
                rawSTR += Current;
                Position++;
                while (!(Current == '"' && Peek(-1) != '\\') && Current != '\0')
                {
                    if (Current == '\\')
                    {
                        rawSTR += Current;
                        Position++;

                        switch (Current)
                        {
                            case '"': str += "\""; break;
                            case 'n': str += "\n"; break;
                            case '\\': str += "\\"; break;
                            case '0': str += "\0"; break;
                            default: throw new Exception("Invalid escape sequence");
                        }
                        rawSTR += Current;
                        Position++;
                    }
                    else
                    {
                        str += Current;
                        rawSTR += Current;
                        Position++;
                    }
                }
                rawSTR += Current;
                return new FToken<EToken>(strToken, startPos, str, rawSTR);
            }
        }
        public class NumberToken<EToken> : LogicalToken<EToken> where EToken : System.Enum
        {
            public NumberToken(EToken intToken, EToken floatToken) { this.intToken = intToken; this.floatToken = floatToken; }
            EToken intToken = default;
            EToken floatToken = default;
            public override bool IfMatch()
            {
                str = string.Empty;
                startPos = Position;
                return char.IsDigit(Current);
            }

            public override FToken<EToken>? ParseToken()
            {

                bool isDecimal = false;

                while ((char.IsDigit(Current) || Current == '.') && Current != '\0')
                {
                    str += Current;

                    if (Current == '.')
                    {
                        isDecimal = true;
                    }

                    Position++;
                }
                Position--;
                if (isDecimal)
                {
                    if (!float.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out float floatVal)) throw new Exception("Invalid number (tried to parse " + str + " as float)");
                    return new(floatToken, startPos, floatVal, str);

                }
                else
                {
                    if (!int.TryParse(str, out int intVal)) throw new Exception("Invalid number!");
                    return new(intToken, startPos, intVal, str);
                }
            }
        }
        public class IdentifierOrKeywordToken<EToken> : LogicalToken<EToken> where EToken : System.Enum
        {
            public IdentifierOrKeywordToken(EToken keywordToken, EToken identifierToken, List<string>? keywords = null)
            {
                this.keywordToken = keywordToken;
                this.identifierToken = identifierToken;
                this.Keywords = keywords ?? new List<string>();
            }
            EToken keywordToken = default;
            EToken identifierToken = default;
            List<string> Keywords = new List<string>();

            public override bool IfMatch()
            {
                str = string.Empty;
                startPos = Position;
                return (char.IsLetter(Current) || Current == '_');
            }

            public override FToken<EToken>? ParseToken()
            {

                while (Current != '\0' && Current != ' ' && (char.IsLetterOrDigit(Current) || Current == '_'))
                {
                    str += Current;
                    Position++;
                }
                Position--;

                var token = new FToken<EToken>(identifierToken, startPos, str, str);

                if (Keywords.Contains(str))
                {
                    token.Type = keywordToken;
                }

                return token;
            }
        }
        public class WhitespaceToken<EToken> : LogicalToken<EToken> where EToken : System.Enum
        {
            public override bool IfMatch()
            {
                str = string.Empty;
                startPos = Position;
                return char.IsWhiteSpace(Current);
            }
            public override FToken<EToken>? ParseToken()
            {
                while (char.IsWhiteSpace(Current))
                {
                    Position++;
                }
                Position--;
                return null;
            }
        }
        public class SinglelineCommentToken<EToken> : LogicalToken<EToken> where EToken : System.Enum
        {
            public SinglelineCommentToken(EToken commentToken) { this.commentToken = commentToken; }
            EToken commentToken = default;
            public override bool IfMatch()
            {
                str = string.Empty;
                startPos = Position;

                bool success = (Current == '/' && Peek(1) == '/');
                try
                {
                    return success;
                }
                finally
                {
                    if (success)
                        Position += 2;
                }
            }

            public override FToken<EToken>? ParseToken()
            {
                while (!(Current == '\n' || Current == '\r' || Current == '\0'))
                {
                    str += Current;
                    Position++;
                }
                return new(commentToken, startPos, str, $"//{str}");
            }
        }
        public class MultilineCommentToken<EToken> : LogicalToken<EToken> where EToken : System.Enum
        {
            public MultilineCommentToken(EToken commentToken) { this.commentToken = commentToken; }
            EToken commentToken = default;
            public override bool IfMatch()
            {
                str = string.Empty;
                startPos = Position;

                bool success = (Current == '/' && Peek(1) == '*');
                try
                {
                    return success;
                }
                finally
                {
                    if (success)
                        Position += 2;
                }
            }


            public override FToken<EToken>? ParseToken()
            {
                while (!((Current == '*' && Peek(1) == '/') || Current == '\0'))
                {
                    str += Current;
                    Position++;
                }
                Position++; //counter in the peek we did
                return new(commentToken, startPos, str, $"/*{str}*/");
            }
        }
    }
}
