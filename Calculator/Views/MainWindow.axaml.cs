using System;
using System.Data;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Developful.Parser;

namespace Developful.Views;

public partial class MainWindow : Window {
    public readonly IBrush? ForegroundBrush;
    
    public MainWindow() {
        InitializeComponent();
        
        NameScope thisWindowNameScope = (NameScope)this.FindNameScope()!; 
        expressionBox = (TextBox)thisWindowNameScope.Find("expressionBox")!;
        resultBox = (TextBox)thisWindowNameScope.Find("resultBox")!;

        ForegroundBrush = resultBox.Foreground;
    }

    private void Calculate(object? sender, RoutedEventArgs e) {
        if (expressionBox.Text == null) return;

        resultBox.Foreground = ForegroundBrush;
        Parser.Calculator calculator = new Parser.Calculator(expressionBox.Text);

        try { resultBox.Text = $"Result: {calculator.Calculate()}."; }
        catch (Exception exception) when (exception is CalculatorException ||
                                          exception is SyntaxErrorException) {
            resultBox.Foreground = Brushes.Red;
            resultBox.Text = exception.Message;
        }
    }
}