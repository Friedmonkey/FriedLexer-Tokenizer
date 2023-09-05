using FriedLexer;
using static FriedLexer.LogicalTokens;

namespace FriedLexerConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string code = """
            var lol = "Hello World";
""";
            FLexer<Token> tokenizer = new FLexer<Token>(code, Token.BadToken, Token.EOF);
            tokenizer.DefinedTokens = new Dictionary<string, Token>
            {
                {";",Token.Semicolon},
                {":",Token.Colon},
                {"=",Token.Equals},
                {"==",Token.EqualsEquals},
                { "=>",Token.Arrow},
            };


            var strToken = new StringToken<Token>(Token.String);
            var singleCommentToken = new SinglelineCommentToken<Token>(Token.Comment);
            var MultiCommentToken = new MultilineCommentToken<Token>(Token.Comment);
            var KeywordToken = new IdentifierOrKeywordToken<Token>(Token.Keyword, Token.Identifier, Keywords);


            tokenizer.AddLogicalToken<WhitespaceToken<Token>>();
            tokenizer.AddLogicalToken(strToken);
            tokenizer.AddLogicalToken(singleCommentToken);
            tokenizer.AddLogicalToken(MultiCommentToken);
            tokenizer.AddLogicalToken(KeywordToken);

            var TokenResult = tokenizer.Lex();


            foreach (var token in TokenResult)
            {
                if (token.Type.Equals(Token.BadToken))
                {
                    Console.WriteLine($"bad token:	on pos:{token.Position,-3} token:{token.Text,-15} with text:{token.Text,-20} with val:{token.Value ?? "Null",-20}");
                }
                else
                {
                    Console.WriteLine($"good token:	on pos:{token.Position,-3} token:{token.Type.GetName(),-15} with text:{token.Text,-20} with val:{token.Value ?? "Null",-20}");
                }
            }
            Console.ReadLine();
        }
        public static List<string> Keywords = new List<string>()
        {
            "var",
            "if",
            "else",
        };
        enum Token
        {
            Semicolon,                  //  ;
            Colon,                      //  :
            Equals,                     //  =
            EqualsEquals,               //  ==
            Arrow,                      //  =>

            String,                     // "anything here"

            Comment,                    // //comment or /* comment */

            Keyword,                    // var if else ( as long as its in list)
            Identifier,                 // myVarible (not in list)

            BadToken,                   //  Token not found
            EOF,                        //  End of the file
        }
    }
}