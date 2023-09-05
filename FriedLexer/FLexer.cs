namespace FriedLexer
{
    public class FLexer<TokenEnum> : AnalizerBase<char> where TokenEnum : System.Enum
    {
        public char EndOfFile = '\0';

        public FLexer(string TextToAnalize, TokenEnum BadToken, TokenEnum EndOfFileToken, char EndOfFile = '\0') : base(TextToAnalize.ToList(), EndOfFile) 
        {
            this.EndOfFile = EndOfFile;
            this.BadToken = BadToken;
            this.EndOfFileToken = EndOfFileToken;
        }
        public FLexer(TokenEnum BadToken, TokenEnum EndOfFileToken, char EndOfFile = '\0') : base(EndOfFile) 
        {
            this.EndOfFile = EndOfFile;
            this.BadToken = BadToken;
            this.EndOfFileToken = EndOfFileToken;
        }

        public Dictionary<string, TokenEnum> DefinedTokens = new Dictionary<string, TokenEnum>();
        public List<LogicalToken<TokenEnum>> DefinedLogicalTokens = new List<LogicalToken<TokenEnum>>();

        public TokenEnum BadToken { get; set; } = default;
        public TokenEnum EndOfFileToken { get; set; } = default;


        public bool AddLogicalToken<logicToken>(string code = null) where logicToken : LogicalToken<TokenEnum>, new()
        {
            try
            {
                LogicalToken<TokenEnum> token = new logicToken();
                token.Analizable = code?.ToList() ?? Analizable;
                token.Position = Position;
                //token.Lock = true;

                DefinedLogicalTokens.Add(token);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool AddLogicalToken<logicToken>(logicToken token, string code = null) where logicToken : LogicalToken<TokenEnum>
        {
            try
            {
                token.Analizable = code?.ToList() ?? Analizable;
                token.Position = Position;
                //token.Lock = true;

                DefinedLogicalTokens.Add(token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SetText(string newText) 
        {
            var charList = newText.ToList();
            foreach (var logicalToken in DefinedLogicalTokens)
            {
                logicalToken.Analizable = charList;
            }
            this.Analizable = charList;
        }

        public List<FToken<TokenEnum>> Lex()
        {
            List<FToken<TokenEnum>> tokens = new List<FToken<TokenEnum>>();
            while (Current != EndOfFile)
            {
                //seems to work great
                FToken<TokenEnum> token = new FToken<TokenEnum>(BadToken, Position, null, Current.ToString(), "BAD");

                //to optimize we only look as many characters forward as we have (depth)
                //we dont have to look 5 characters if our longest matchable is only 4 characters
                int depth = DefinedTokens.Keys.GetLongestLength();
                int startPos = Position;
                for (int i = 0; i < depth; i++)
                {
                    var start = string.Empty;

                    //construct/build the string that we will be looking for
                    for (int j = -i; j <= 0; j++)
                    {
                        start += $"{Peek(j)}";
                    }

                    var matching = DefinedTokens.Where(t => t.Key.StartsWith(start)).ToList();
                    if (matching.Count() > 0)
                    {
                        var first = matching.First();
                        if (first.Key == start)
                        {
                            token = new FToken<TokenEnum>(first.Value, startPos, null, first.Key);
                            // if there is only one match we dont need to look further
                            if (matching.Count() == 1)
                                break;
                        }
                    }
                    else
                    {
                        //if we didnt match, we have to go back
                        Position = Position - i;
                        //depending on how long the token was of cource
                        if (!token.isBadToken)
                        {
                            Position += token.Text.Length - 1; //probally some flaws here but i guess ill fix later, i heavent run into anything yet
                        }
                        break;
                    }
                    Position++;
                }

                //match logical tokens

                if (token.isBadToken)
                {
                    foreach (var logicalDefinition in DefinedLogicalTokens)
                    {
                        if (!token.isBadToken) //if we already macthed it we dont need to look further/try to overwrite it
                            break;

                        logicalDefinition.Position = Position; //update the position
                        if (logicalDefinition.IfMatch())
                        {
                            //returning null will just set it as a null token which means it gets ignored all together
                            //think of a whitespace token, which can be ignored all together
                            token = logicalDefinition.ParseToken() ?? new FToken<TokenEnum>(BadToken, "NULL");
                            Position = logicalDefinition.Position; //if it matched we need to update our position
                        }
                    }
                }
                if (!token.isNull)
                    tokens.Add(token);
                Position++;
            }

            //end of the file, we're done here
            tokens.Add(new FToken<TokenEnum>(EndOfFileToken, Position, null, "<EOF>", "EOF"));

            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];
                if (token.isBadToken)
                {
                    tokens[i] = new FToken<TokenEnum>(token, BadToken);
                }
                else if (token.isEndOfFile)
                {
                    tokens[i] = new FToken<TokenEnum>(token, EndOfFileToken);
                }
            }

            return tokens;
        }
    }
}