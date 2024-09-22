# FriedLexer-Tokenizer
Simple tokenizer to turn any text into a bunch of tokens, and all you really have to do is define the tokens themselves

there are 2 categories of tokens
static tokens, they are always the same like `;` or `=`

and dynamic/logical tokens these tokens are diffrent but they are a `concept`
think of a string or a number or a comment or a keyword
`"Hello world"` and `"Bye world"`
are both strings but they are diffrent, this means it requires some logic to see if its a string or not

same with numbers
`79` and '42' are both numbers but they're diffrent

Take a look at `Program.cs` in `FriedLexerConsole` for an example

you can see they use default logical tokens but you can also change or make your own logical tokens if you need to

this is pretty much all the code needed to define them
```
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
```

but of course you need to define the enums (and keywords) as well

so you need

```
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

```


keep in mind i didnt really think when writing this code, so its pretty bad but it seems to work fine for me and im quite happy with it
if you have any suggestions, improvements or feedback
let me know somehow (i dont know how to use github)
