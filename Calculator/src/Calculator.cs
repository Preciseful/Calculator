using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using Developful.Calculator.Expressions;

namespace Developful.Parser;

public class CalculatorException : Exception {
    public CalculatorException(string message) : base(message) { }
}

public class Calculator {
    private string content;

    public Calculator(string content) {
        this.content = content;
    }

    [Pure]
    public double Calculate() {
        if (content.Trim() == "") return 0;
        
        List<Token> tokens = Tokenize();
        Parser parser = new Parser(tokens);

        Expression expr = parser.Parse();
        return HandleExpression(expr);
    }

    [Pure]
    private List<Token> Tokenize() {
        List<Token> tokens = new List<Token>();
        Regex regex = new Regex(@"([0-9]+(?:\.[0-9]*)?|[+\-\*\/^V]|\(\))|([^\s])");

        MatchCollection collection = regex.Matches(content);
        foreach (Match match in collection) {
            if (char.IsDigit(match.Value[0])) 
                tokens.Add(new Token(match.Value, "number"));
            else if(match.Value == "(")
                tokens.Add(new Token(match.Value, "lparan"));
            else if(match.Value == ")")
                tokens.Add(new Token(match.Value, "rparan"));
            else if (match.Groups[2].Value != "")
                throw new CalculatorException($"Invalid character '{match.Value}'.");
            else 
                tokens.Add(new Token(match.Value, "operator"));
        }

        tokens.Add(new Token("EOF", "EOF"));
        return tokens;
    }
    
    [Pure]
    private double HandleExpression(Expression expression) =>
        expression switch {
            BinaryExpression binary => HandleBinary(binary),
            UnaryExpression unary => HandleUnary(unary),
            Number number => number.Value,
            _ => 0
        };

    private double HandleBinary(BinaryExpression expression) {
        double lhv = HandleExpression(expression.Lhv), 
            rhv = HandleExpression(expression.Rhv);

        return expression.Operator.Kind switch {
            Operator.SyntaxKind.Plus => lhv + rhv,
            Operator.SyntaxKind.Minus => lhv - rhv,
            Operator.SyntaxKind.Divide => lhv / rhv,
            Operator.SyntaxKind.Multiply => lhv * rhv,
            Operator.SyntaxKind.Pow => Double.Pow(lhv, rhv),
            Operator.SyntaxKind.Sqrt => lhv * Double.Sqrt(rhv),
            _ => throw new ArgumentException("Invalid syntax kind for operator.", "expression")
        };
    }
    
    private double HandleUnary(UnaryExpression expression) {
        double value = HandleExpression(expression.Value);

        return expression.Operator.Kind switch {
            Operator.SyntaxKind.Sqrt => Double.Sqrt(value),
            Operator.SyntaxKind.Minus => -value,
            _ => throw new ArgumentException("Invalid syntax kind for operator.", "expression")
        };
    }
}