namespace Lexemes;

public class TextScanner(string str)
{
    private readonly string _str = str;

    private int _position;

    public char Peek(int n = 0)
    {
        int postiton = _position + n;
        return postiton >= _str.Length ? '\0' : _str[postiton];
    }

    public void Advance()
    {
        _position++;
    }

    /// <summary>
    /// Проверяет, достигли ли мы конца входных данных.
    /// </summary>
    public bool IsEnd()
    {
        return _position >= _str.Length;
    }
}