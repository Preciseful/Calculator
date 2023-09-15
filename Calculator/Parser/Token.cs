namespace Developful.Parser; 

public class Token {
    public string Symbol;
    public string Type;

    public Token(string symbol, string type) {
        this.Symbol = symbol;
        this.Type = type;
    }
}