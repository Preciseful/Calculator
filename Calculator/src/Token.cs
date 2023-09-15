namespace Developful.Parser; 

public class Token {
    public string Symbol;
    public string Type;

    public Token(string symbol, string type) {
        this.Symbol = symbol;
        this.Type = type;
    }

    public static bool operator ==(Token x, Token y) => x.Type == y.Type;
    public static bool operator !=(Token x, Token y) => x.Type != y.Type;
    public static bool operator ==(Token x, string y) => x.Type == y;
    public static bool operator !=(Token x, string y) => x.Type != y;
}