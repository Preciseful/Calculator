using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

namespace Developful.Parser;

public class CalculatorException : Exception {
    public CalculatorException(string message) : base(message) { }
}

public class Calculator {
    private string content;
    
    public Calculator(string content) {
        this.content = content;
    }

    public void Parse() {
        List<Token> tokens = Tokenize();
        Term();
    }

    [Pure]
    private List<Token> Tokenize() {
        List<Token> tokens = new List<Token>();
        Regex regex = new Regex(@"([0-9]+(?:\.[0-9]*)?|[+\-\*\/^V])|([^\s])");

        MatchCollection collection = regex.Matches(content);
        foreach (Match match in collection) {
            if (char.IsDigit(match.Value[0])) 
                tokens.Add(new Token(match.Value, "number"));
            else if (match.Groups[2].Value != "")
                throw new CalculatorException($"Invalid token '{match.Value}'.");
            else 
                tokens.Add(new Token(match.Value, "operator"));
        }

        return tokens;
    }

    private void Term() {
        
    }
}