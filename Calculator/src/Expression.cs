using System.Data;
using Developful.Parser;

namespace Developful.Calculator.Expressions; 

public class Expression {
    public Token Source;

    public Expression(Token source) => this.Source = source;
}

public class Operator {
    public enum SyntaxKind {
        Plus,
        Minus,
        Divide,
        Multiply,
        Pow,
        Sqrt
    }

    public SyntaxKind Kind;

    public Operator(SyntaxKind kind) => this.Kind = kind;

    public Operator(Token token) {
        this.Kind = token.Symbol switch {
            "+" => SyntaxKind.Plus,
            "-" => SyntaxKind.Minus,
            "*" => SyntaxKind.Multiply,
            "/" => SyntaxKind.Divide,
            "^" => SyntaxKind.Pow,
            "V" => SyntaxKind.Sqrt,
            _ => throw new SyntaxErrorException($"Invalid operator '{token.Symbol}'.")
        };
    }
}

public class UnaryExpression : Expression {
    public Operator Operator;
    public Expression Value { get; private set; }

    public UnaryExpression(Token source, Operator @operator, Expression value) : base(source) {
        this.Operator = @operator;
        this.Value = value;
    }
}

public class BinaryExpression : Expression {
    public Operator Operator;
    public Expression Lhv { get; private set; }
    public Expression Rhv { get; private set; }

    public BinaryExpression(Token source, Expression lhv, Operator @operator, Expression rhv) : base(source) {
        this.Lhv = lhv;
        this.Operator = @operator;
        this.Rhv = rhv;
    }
}

public class Number : Expression {
    public double Value;

    public Number(Token source, double value) : base(source) => this.Value = value;
    public Number(Token source) : base(source) => this.Value = double.Parse(source.Symbol);
    
}

