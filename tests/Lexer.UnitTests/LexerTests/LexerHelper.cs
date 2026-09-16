namespace Lexemes.UnitTests;

public static class LexerHelper
{
    public static List<Token> Tokenize(string code)
    {
        List<Token> results = new();
        Lexemes.Lexer lexer = new(code);

        for (Token t = lexer.ParseToken(); t.Type != TokenType.EndOfFile; t = lexer.ParseToken())
        {
            results.Add(t);
        }

        return results;
    }
}