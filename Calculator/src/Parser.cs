using System.Collections.Generic;
using System.Data;
using System.Linq;
using Developful.Calculator.Expressions;

namespace Developful.Parser; 

public class Parser {
    private List<Token> tokens;
    private int _pos;
    private Token current => tokens.ElementAtOrDefault(_pos) ?? tokens.Last();
    private bool ongoing => current == "EOF";

    private readonly string[] termTypes = new[] { "+", "-" };
    private readonly string[] factorTypes = new[] { "*", "/", "V" };

    public Parser(List<Token> tokens) {
        this.tokens = tokens;
    }
    
    private Token eat(string type, string error) {
        if (current != type) throw new SyntaxErrorException(error);
        Token ret = current;
        _pos++;
        return ret;
    }
    
    public Expression Parse() => Term();

    public Expression Term() {
        Expression expr = Factor();
        
        while (termTypes.Contains(current.Symbol)) {
            Token op = eat("operator", "Expected an operator.");
            expr = new BinaryExpression(op, expr, new Operator(op), Factor());
        }

        return expr;
    }

    public Expression Factor() {
        Expression expr = Pow();
        
        while (factorTypes.Contains(current.Symbol)) {
            Token op = eat("operator", "Expected an operator.");
            
            // the user can do 3V3 instead of 3 * V3
            if (current.Symbol == "V") {
                expr = new BinaryExpression(
                    op, 
                    expr, 
                    new Operator(op), 
                    new UnaryExpression(op, new Operator(op), Primary()));
                
                continue;
            }

            expr = new BinaryExpression(op, expr, new Operator(op), Pow());
        }

        return expr;
    }
    
    public Expression Pow() {
        Expression expr = Primary();
        
        while (current.Symbol == "^") {
            Token op = eat("operator", "Expected an operator.");
            expr = new BinaryExpression(op, expr, new Operator(op), Primary());
        }

        return expr;
    }

    public Expression Primary() {
        if (current.Symbol == "-" || current.Symbol == "V") {
            Token op = eat("operator", "Expected an operator.");
            return new UnaryExpression(op, new Operator(op), Primary());
        }
        
        if (current == "lparan") {
            // this error will NEVER occur under normal circumstances but still
            eat("lparan", "Expected a '('.");
            Expression expr = Parse();
            eat("rparan", "Expected a ')'.");
            return expr;
        }

        if (current == "number")
            return new Number(eat("number", "Expected a number."));
        
        throw new SyntaxErrorException("Expected a valid expression.");
    }
}