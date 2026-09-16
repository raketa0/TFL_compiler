namespace Lexemes.UnitTests;

public class CommentTests
{
    [Theory]
    [MemberData(nameof(GetSingleLineComments))]
    public void Can_skip_single_line_comments(string code, List<Token> expected)
    {
        List<Token> actual = LexerHelper.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetSingleLineComments()
    {
        return new TheoryData<string, List<Token>>()
        {
            // одиночный комментарий — пустой результат
            { "# это комментарий", [] },

            // комментарий перед кодом
            {
                """
                # комментарий
                var x
                """,
                [
                    new Token(TokenType.Var),
                    new Token(TokenType.Identifier, new TokenValue("x")),
                ]
            },

            // комментарий после кода в той же строке
            {
                """
                var x # объявление
                var y
                """,
                [
                    new Token(TokenType.Var),
                    new Token(TokenType.Identifier, new TokenValue("x")),
                    new Token(TokenType.Var),
                    new Token(TokenType.Identifier, new TokenValue("y")),
                ]
            },

            // несколько комментариев подряд
            {
                """
                # первый
                # второй
                main
                """,
                [new Token(TokenType.Main)]
            },
        };
    }
}